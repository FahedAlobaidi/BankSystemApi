using BankSystemApi.Entities;
using BankSystemApi.Exceptions;
using BankSystemApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankSystemApi.Controllers
{
    [Route("api/admin")]
    [Authorize("MustBeAdmin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAccountRepo _accountRepo;

        public AdminController(IAccountRepo accountRepo)
        {
            _accountRepo = accountRepo;
        }

        [HttpPatch("changeStatus/{status}/{accountNumber}")]
        public async Task<IActionResult> ChangeStatus(string status,string accountNumber)
        {
            var accountEnt =await _accountRepo.GetAccountByAccountNumber(accountNumber);

            if (accountEnt == null)
            {
                throw new NotFoundException("There is no account with this account number, please enter correct account number");
            }

            var accountStatusType = accountEnt.Status.ToString().ToLower();

            if (status == accountStatusType)
            {
                throw new BadRequestException("The account is already with this status type");
            }

            //if (status == "active")
            //{
            //    accountEnt.Status = Account.StatusTypes.Active;
            //}else if (status == "frozen")
            //{
            //    accountEnt.Status = Account.StatusTypes.Frozen;
            //}else if (status == "closed")
            //{
            //    accountEnt.Status=Account.StatusTypes.Closed;
            //}
            //else
            //{
            //    throw new BadRequestException("Enter the right status");
            //}

            if(Enum.TryParse<Account.StatusTypes>(status,ignoreCase:true,out var newStatus))
            {
                accountEnt.Status = newStatus;
            }
            else
            {
                throw new BadRequestException("Enter the right status");
            }

            await _accountRepo.SaveChanges();

            return Ok("Status has been changed");
        }

        //this to test the claims
        [HttpGet("debug-claims")]
        //[Authorize]  // just requires login, no policy
        public IActionResult DebugClaims()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            return Ok(claims);
        }
    }
}
