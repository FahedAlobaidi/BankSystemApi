using AutoMapper;
using BankSystemApi.DbContexts;
using BankSystemApi.Entities;
using BankSystemApi.Exceptions;
using BankSystemApi.Models;
using BankSystemApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using System.Security.Principal;
using System.Transactions;

namespace BankSystemApi.TransactionsServices
{
    public class TransactionService : ITransactionService
    {

        private readonly IAccountRepo _accountRepo;
        private readonly ITransactionRepo _transactionRepo;
        private readonly BankContext _context;
        private readonly IMapper _mapper;

        public TransactionService(IAccountRepo accountRepo,ITransactionRepo transactionRepo,IMapper mapper,BankContext context)
        {
            _accountRepo = accountRepo;
            _transactionRepo = transactionRepo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<TransactionDto>> GetTransactions(Guid clientId,string accountNumber)
        {
            var accountEnt = await _accountRepo.GetAccountByAccountNumber(accountNumber);

            if (accountEnt == null)
            {
                throw new NotFoundException($"there are no account with this number {accountNumber}");
            }

            if(accountEnt.ClientId!= clientId)
            {
                throw new BadRequestException("You don`t have this account number");
            }

            var transactions = await _transactionRepo.GetTransactionsAsync(accountEnt.Id);

            return _mapper.Map<IEnumerable<TransactionDto>>(transactions);
        }

        public async Task<TransactionDto> MakeDeposit(Guid clientId,string accountNumber, decimal amount)
        {
            using var transactionDb = await _context.Database.BeginTransactionAsync();

            try
            {


                if (amount <= 0)
                {
                     throw new BadRequestException("Amount must be positive");
                }

                var accountEnt = await _accountRepo.GetAccountByAccountNumber(accountNumber);

                if (accountEnt == null)
                {
                    throw new NotFoundException("The account with this id is not exist");
                }

                if(accountEnt.ClientId!= clientId)
                {
                    throw new BadRequestException("You don`t have this account number");
                }

                if (accountEnt.Status == Account.StatusTypes.Closed)
                {
                    throw new BadRequestException("Your account is closed so you cant make this transaction");
                }

                accountEnt.AccountBalance = accountEnt.AccountBalance + amount;

                var transaction = new Entities.Transaction
                {
                    AccountId = accountEnt.Id,
                    RunningBalance = accountEnt.AccountBalance,
                    TransactionType = Entities.TransactionType.Deposit,
                    Description = "Deposit to my account",
                    Currency="JOD",
                    Amount=amount
                };

                _transactionRepo.AddTransaaction(transaction);

                await _transactionRepo.SaveChanges();

                await transactionDb.CommitAsync();

                return _mapper.Map<TransactionDto>(transaction);
            }
            catch
            {
                await transactionDb.RollbackAsync();
                throw;
            }
        }

        public async Task<TransactionDto> Withdrawal(Guid clientId,string accountNumber, decimal amount)
        {

            using var transactionDb = await _context.Database.BeginTransactionAsync();


            try
            {
                var account = await _accountRepo.GetAccountByAccountNumber(accountNumber);

                if (account == null)
                {
                    throw new NotFoundException("Wrong account number");
                }

                if (account.ClientId != clientId)
                {
                    throw new BadRequestException("You don`t have this account number");
                }

                if (account.Status == Account.StatusTypes.Closed || account.Status == Account.StatusTypes.Frozen) throw new BadRequestException("Your account is closed or frozen so you cant make this transaction");

                if (account.AccountBalance < amount)
                {
                    throw new BadRequestException("You don`t have enough money");
                }

                account.AccountBalance = account.AccountBalance - amount;

                var transactionEnt = new Entities.Transaction()
                {
                    AccountId = account.Id,
                    Currency="JOD",
                    RunningBalance = account.AccountBalance,
                    TransactionType = Entities.TransactionType.Withdrawal,
                    Description = "Withdrawal",
                    Amount = 0 - amount
                };

                _transactionRepo.AddTransaaction(transactionEnt);

                await _transactionRepo.SaveChanges();

                await transactionDb.CommitAsync();

                return (_mapper.Map<TransactionDto>(transactionEnt));
            }
            catch
            {
                await transactionDb.RollbackAsync();
                throw;//This re-throws the original error (keeping the stack trace) so the Controller can see what actually happened
            }
            
        }

        public async Task<IEnumerable<TransactionDto>> Transfer(Guid clientId,string accountNumberIn, decimal amount, string accountNumberOut)
        {
            using var transactionDb = await _context.Database.BeginTransactionAsync();

            try
            {
                if (amount <= 0)
                {
                    throw new BadRequestException("Amount must be positive");
                }

                if (accountNumberIn == accountNumberOut) throw new BadRequestException("cant transfer to the same account number");

                List<TransactionDto> transactions = new List<TransactionDto>();

                var recieverAccountEnt = await _accountRepo.GetAccountByAccountNumber(accountNumberIn);

                if (recieverAccountEnt == null)
                {
                    throw new NotFoundException($"The account with this {accountNumberIn} is not exist");
                }

                var senderAccountEnt = await _accountRepo.GetAccountByAccountNumber(accountNumberOut);

                if (senderAccountEnt == null)
                {
                    throw new NotFoundException($"The account with this {accountNumberOut} is not exist");
                }

                if (senderAccountEnt.ClientId != clientId)
                {
                    throw new BadRequestException("You don`t have this account number");
                }
                if (senderAccountEnt.Status == Account.StatusTypes.Closed || senderAccountEnt.Status == Account.StatusTypes.Frozen) throw new BadRequestException("Your account is closed or frozen so you cant make this transaction");

                if (amount > senderAccountEnt.AccountBalance)
                {
                    throw new BadRequestException("your account don`t have this amount");
                }



                recieverAccountEnt.AccountBalance = recieverAccountEnt.AccountBalance + amount;

                var transactionIn = new Entities.Transaction
                {
                    AccountId = recieverAccountEnt.Id,
                    RunningBalance = recieverAccountEnt.AccountBalance,
                    TransactionType = Entities.TransactionType.TransferIn,
                    Description = $" transfer to {accountNumberIn} this {amount}",
                    Currency = "JOD",
                    Amount = amount,
                };

                transactions.Add(_mapper.Map<TransactionDto>(transactionIn));

                senderAccountEnt.AccountBalance = senderAccountEnt.AccountBalance - amount;

                var transactionOut = new Entities.Transaction
                {
                    AccountId = senderAccountEnt.Id,
                    RunningBalance = senderAccountEnt.AccountBalance,
                    TransactionType = Entities.TransactionType.TransferOut,
                    Description = $"transfer from {accountNumberOut} this {amount}",
                    Currency = "JOD",
                    Amount = 0 - amount
                };

                transactions.Add(_mapper.Map<TransactionDto>(transactionOut));


                _transactionRepo.AddTransaaction(transactionIn);
                _transactionRepo.AddTransaaction(transactionOut);

                await _transactionRepo.SaveChanges();

                await transactionDb.CommitAsync();

                return transactions;
            }
            catch
            {
                await transactionDb.RollbackAsync();
                throw;
            }

        }

        
    }
}
