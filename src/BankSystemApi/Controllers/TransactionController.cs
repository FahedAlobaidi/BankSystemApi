using BankSystemApi.Models;
using BankSystemApi.TransactionsServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemApi.Controllers
{
    [Route("api/transactions")]
    [Authorize]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        
        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("deposit")]
        public async Task<ActionResult<TransactionDto>> MakeDeposit([FromBody] DepositDto depositDto)
        {
            var clientIdString = User.FindFirst("sub_client")?.Value;

            if (string.IsNullOrEmpty(clientIdString))
            {
                return BadRequest("Token is invalid: No Client ID found.");
            }

            var clientId = Guid.Parse(clientIdString);

            var amount = depositDto.Amount;
            var accountNum = depositDto.AccountNumber;

            var transactionDto = await _transactionService.MakeDeposit(clientId,accountNum, amount);

            return Ok(transactionDto);
        }

        [HttpPost("withdrawal")]
        public async Task<ActionResult<TransactionDto>> Withdrawal(DepositDto withdrawal)
        {
            var clientIdString = User.FindFirst("sub_client")?.Value;

            if (string.IsNullOrEmpty(clientIdString))
            {
                return BadRequest("Token is invalid: No Client ID found.");
            }


            var clientId = Guid.Parse(clientIdString);

            var transactionDto = await _transactionService.Withdrawal(clientId,withdrawal.AccountNumber, withdrawal.Amount);

            return Ok(transactionDto);
        }

        [HttpPost("transfer")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> Transfer([FromBody] TransferDto transferDto)
        {
            var clientIdString = User.FindFirst("sub_client")?.Value;

            if (string.IsNullOrEmpty(clientIdString))
            {
                return BadRequest("Token is invalid: No Client ID found.");
            }

            var clientId = Guid.Parse(clientIdString);

            var transferTransactions = await _transactionService.Transfer(clientId,transferDto.AccountNumberIn, transferDto.Amount, transferDto.AccountNumberOut);

            return Ok(transferTransactions);
        }

        [HttpGet("{accountNumber}")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions(string accountNumber)
        {
            var clientIdString = User.FindFirst("sub_client")?.Value;

            if (string.IsNullOrEmpty(clientIdString))
            {
                return BadRequest("Token is invalid: No Client ID found.");
            }

            var clientId = Guid.Parse(clientIdString);

            var transactions = await _transactionService.GetTransactions(clientId,accountNumber);

            return  Ok(transactions);
        }

    }
}
