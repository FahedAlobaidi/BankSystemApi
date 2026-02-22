using AutoMapper;
using BankSystemApi.Entities;
using BankSystemApi.Models;
using BankSystemApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

//Ent is a shortcut for Entity

namespace BankSystemApi.Controllers
{
    [Route("api/client")]
    [Authorize]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IClientRepository _clientRepo;
        const int defaultPageSize = 5;

        public ClientController(IMapper mapper,IClientRepository clientRepository)
        {
            _mapper = mapper;
            _clientRepo = clientRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetAllClients(int pageSize=5,int pageNumber=1)
        {
            if (pageSize < defaultPageSize)
            {
                pageSize = pageNumber;
            }

            if (pageNumber < 0)
            {
                pageNumber = 1;
            }

            var allClients = await _clientRepo.GetAllClientsAsync(pageSize,pageNumber);

            return Ok(_mapper.Map<IEnumerable<ClientDto>>(allClients));
        }

        

        [HttpGet("{clientId}",Name ="GetClient")]
        public async Task<ActionResult<ClientDto>> GetClient(Guid clientId)
        {
            var clientEnt = await _clientRepo.GetClientAsync(clientId);

            if (clientEnt == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ClientDto>(clientEnt));
        }

        



        //[HttpPost]
        //public async Task<IActionResult> CreateClient(ClientCreateDto clientCreateDto)
        //{
        //    if (await _clientRepo.IsClientEmailExistAsync(clientCreateDto.Email))
        //    {
        //        return BadRequest();
        //    }

        //    var clientEnt = _mapper.Map<Client>(clientCreateDto);

        //    _clientRepo.AddClientAsync(clientEnt);

        //    await _clientRepo.SaveChanges();

        //    var clientDto = _mapper.Map<ClientDto>(clientEnt);

        //    return CreatedAtRoute("GetClient", new
        //    {
        //        clientId = clientDto.Id,
        //    }, clientDto);
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateClient(ClientCreateDto client)
        //{


        //    var clientEnt = _mapper.Map<Client>(client);



        //    clientEnt.PinCodeHash = BCrypt.Net.BCrypt.HashPassword(client.PinCodeHash);
        //    clientEnt.AccountBalance = 0;//bcs the user cant enter the balance by himself and its must be by trancsactions
        //    clientEnt.AccountNumber = await GenerateAccountNumberAsync();//bcs the cant write the account number we who shold take care of that


        //    _clientRepo.AddClientAsunc(clientEnt);

        //    await _clientRepo.SaveChanges();

        //    var createdClientDto = _mapper.Map<ClientDto>(clientEnt);

        //    return CreatedAtRoute("GetClient", new
        //    {
        //        clientId = clientEnt.Id,
        //    }, createdClientDto);
        //}

        [HttpPatch("{clientId}")]
        public async Task<ActionResult> PartiallyUpdateClient(Guid clientId,JsonPatchDocument<ClientUpdateDto> patchDocument)
        {
            var clientEnt = await _clientRepo.GetClientAsync(clientId);

            if (clientEnt == null)
            {
                return NotFound();
            }

            var clientPatch = _mapper.Map<ClientUpdateDto>(clientEnt);

            patchDocument.ApplyTo(clientPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!TryValidateModel(clientPatch))
            {
                return BadRequest();
            }

            _mapper.Map(clientPatch, clientEnt);//this will update the info 

            await _clientRepo.SaveChanges();

            return NoContent();
        }

        //private async Task<string> GenerateAccountNumberAsync()
        //{
        //    var random = new Random();
        //    string accountNumber;
        //    bool exist=false;

        //    do
        //    {
        //        long number = random.NextInt64(100000000000, 999999999999);
        //        accountNumber = number.ToString();
        //        exist = await _clientRepo.IsAccountNumberExistAsync(accountNumber);

        //    } while (exist);

        //    return accountNumber;
        //}

    }
}
