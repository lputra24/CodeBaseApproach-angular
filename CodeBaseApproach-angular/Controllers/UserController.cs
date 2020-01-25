using CodeBaseApproach_angular.Data;
using CodeBaseApproach_angular.DTO;
using CodeBaseApproach_angular.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CodeBaseApproach_angular.Controllers
{
    [Route("api/[controller]")]
    //allow the MVC to infer where the data is coming from similar with [FromBody]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IAuthRepository _repo;
        public UserController(IAuthRepository repo, IConfiguration config)
        {
            this._repo = repo;
            this._config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> register(UserForRegisterDto userForRegisterDto) {
            //need to validate later
            userForRegisterDto.username = userForRegisterDto.username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.username)) {
                return BadRequest("username already taken");
            }

            var user = new User();
            user.username = userForRegisterDto.username;

            await _repo.Register(user, userForRegisterDto.password);

            return StatusCode(201);
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(UserForLoginDto userForLoginDto)
        {
            var user = await _repo.Login(userForLoginDto.username.ToLower(), userForLoginDto.password);
            if (user == null) {
                return Unauthorized();
            }

            //build JWT token id and username claims
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                new Claim(ClaimTypes.Name,user.username)
            };

            //server needs to sign token

            //security key
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            //use key to sign credentials with hashing algorithm
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //create tokens

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                //expiry
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            //handler to create token
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                //reply client with token
                token = tokenHandler.WriteToken(token)
            }); ;

        }

    }
}
