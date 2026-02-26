using AutoMapper;
using BankSystemApi.Entities;
using BankSystemApi.Exceptions;
using BankSystemApi.Models;
using BankSystemApi.Services;
using BankSystemApi.TokenService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BankSystemApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepo _UserRepo;
        private readonly IClientRepository _clientRepository;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public AuthController(IMapper mapper,IUserRepo userRepo,IClientRepository clientRepository,IConfiguration configuration,ITokenService tokenService)
        {
            _mapper = mapper;
            _UserRepo = userRepo;
            _clientRepository = clientRepository;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        [HttpPost("register")] 
        public async Task<IActionResult> CreateRegistration(RegisterationDto registerationDto)
        {
            if(await _UserRepo.IsEmailExistAsync(registerationDto.Email))
            {
                //this will return plain text and i dont need that i am already added exception handler
                //return BadRequest("The email is already exist");

                throw new BadRequestException("The email is already exist");
            }

            var userEnt = _mapper.Map<User>(registerationDto);

            userEnt.Id = Guid.NewGuid();
            userEnt.PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(registerationDto.PasswordHash);

            _UserRepo.AddUser(userEnt);

            var clientEnt = _mapper.Map<Client>(registerationDto);

            clientEnt.UserId = userEnt.Id;

            _clientRepository.AddClientAsync(clientEnt);

            try
            {
                await _UserRepo.SaveChanges();

                return Ok("User registered successfully");
            }
            catch(Exception x)
            {
                return StatusCode(500, "Something went wrong during registration");
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            bool isEmailExist = await _UserRepo.IsEmailExistAsync(loginDto.Email);

            
            if (!isEmailExist)
            {
                //return Unauthorized("You write the wrong password or email");
                throw new UnauthorizedException("You write the wrong password or email");
            }

            var user = await _UserRepo.GetUserWithClientInfo(loginDto.Email);

            bool isPasswordValid = BCrypt.Net.BCrypt.EnhancedVerify(loginDto.PasswordHash, user?.PasswordHash);

            if (user.Client == null)
            {
                return BadRequest("No client with this email");
            }

            
            if (!isPasswordValid)
            {
                //return Unauthorized("You write the wrong password or email");
                throw new BadRequestException("You write the wrong password or email");
            }

            var returnedToken = _tokenService.CreateToken(user);

            return Ok(new {token=returnedToken});
        }
    }
}
