using System.Threading.Tasks;
using datingapp.api.DataContext;
using datingapp.api.DTO;
using datingapp.api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace datingapp.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo,IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDtos userForRegisterDtos)
        {
            //validate request
            userForRegisterDtos.Username= userForRegisterDtos.Username.ToLower();
            if(await _repo.UserExists(userForRegisterDtos.Username))
            {
                return BadRequest("Username Already Exists");
            }

            var userToCreate = new User
            {
                UserName=userForRegisterDtos.Username
            };

            var createdUser =await _repo.Register(userToCreate,userForRegisterDtos.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDtos userForLoginDtos)
        {
            var userFromRepo =await _repo.Login(userForLoginDtos.Username.ToLower(),userForLoginDtos.Password);
            if(userFromRepo==null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.id.ToString()),
                new Claim(ClaimTypes.Name,userFromRepo.UserName)
            };
            var key= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var cred =new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor =new SecurityTokenDescriptor
            {
                Subject= new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials=cred
                
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token=tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {token=tokenHandler.WriteToken(token)});  
        }
    }
}