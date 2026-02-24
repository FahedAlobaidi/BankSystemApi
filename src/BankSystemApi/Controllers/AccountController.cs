using AutoMapper;
using BankSystemApi.Entities;
using BankSystemApi.Models;
using BankSystemApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemApi.Controllers
{
    [Route("api/accounts")]
    [Authorize]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAccountRepo _accountRepo;

        public AccountController(IMapper mapper, IAccountRepo accountRepo)
        {
            _mapper = mapper;
            _accountRepo = accountRepo;
        }

        [HttpGet("accountId/{accountId}")]
        public async Task<ActionResult<AccountInfoDto>> GetAccount(Guid accountId)
        {
            var accountEnt = await _accountRepo.GetAccountById(accountId);

            if (accountEnt == null)
            {
                return NotFound();
            }

            var accountDto = _mapper.Map<AccountInfoDto>(accountEnt);

            return Ok(accountDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountInfoDto>>> GetAccounts()
        {
            var clientIdString = User.FindFirst("sub_client")?.Value;

            if (string.IsNullOrEmpty(clientIdString))
            {
                return BadRequest("Token is invalid: No Client ID found.");
            }

            Guid clientId = Guid.Parse(clientIdString);

            IEnumerable<Account?> accounts = await _accountRepo.GetAccountByClientIdAsync(clientId);

            if (accounts == null) return BadRequest("You don`t have any accounts yet");

            return Ok(_mapper.Map<IEnumerable<AccountInfoDto>>(accounts));
        }

        [HttpGet("accountnumber/{accountNumber}")]
        public async Task<ActionResult<AccountInfoDto>> GetAccountByAccountNumber(string accountNumber)
        {
            //first i check the is the acc exist 
            //second i get the data from the database
            //third i transform it to dto and send it back

            if (!await _accountRepo.IsAccountNumberExistAsync(accountNumber))
            {
                return NotFound();
            }

            var accountEnt =await _accountRepo.GetAccountByAccountNumber(accountNumber);

            return  Ok(_mapper.Map<AccountInfoDto>(accountEnt));

        }

        [HttpPost("createAccount")]
        public async Task<IActionResult> CreateAccount(AccountCreateDto accountCreateDto)
        {
            var clientIdString = User.FindFirst("sub_client")?.Value;

            if (string.IsNullOrEmpty(clientIdString))
            {
                return BadRequest();
            }

            var clientId = Guid.Parse(clientIdString);

            var accountEnt = _mapper.Map<Account>(accountCreateDto);

            accountEnt.AccountNumber = await GenerateAccountNumberAsync();

            accountEnt.ClientId = clientId;

            _accountRepo.AddAccount(accountEnt);

            await _accountRepo.SaveChanges();

            return Ok("Account has been added");
        }



        private async Task<string> GenerateAccountNumberAsync()
        {
            var random = new Random();
            string accountNumber;
            bool exist = false;

            do
            {
                long number = random.NextInt64(100000000000, 999999999999);
                accountNumber = number.ToString();
                exist = await _accountRepo.IsAccountNumberExistAsync(accountNumber);

            } while (exist);

            return accountNumber;
        }
    }
}
