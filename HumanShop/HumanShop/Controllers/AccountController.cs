using HumanShop.Data;
using HumanShop.DTOs;
using HumanShop.Entities;
using HumanShop.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace HumanShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(ApplicationDBContext dBContext, ITokenService tokenService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> CreateAccount([FromBody] RegisterDTO register)
        {
            if(dBContext.appUsers.FirstOrDefault(x => x.UserName.ToLower() == register.Username.ToLower()) != null)
            {
                return BadRequest("Username is exist");
            }
            //using var hmac = new HMACSHA512();
            //var user = new AppUser {
            //    UserName = register.Username,
            //    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.Password)),
            //    PasswordSalt = hmac.Key
            //};
            //dBContext.appUsers.Add(user);
            //await dBContext.SaveChangesAsync();
            //return new UserDTO
            //{
            //    Username = user.UserName,
            //    Token = tokenService.CreateToken(user)
            //};
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginDTO login)
        {
            var user = await dBContext.appUsers.FirstOrDefaultAsync(x => x.UserName == login.Username);
            if(user == null) {
                return Unauthorized("Invalid username");
            }
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(login.Password));
            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != user.PasswordHash[i]) {
                    return Unauthorized("Invalid password");
                }
            }
            return new UserDTO
            {
                Username = user.UserName,
                Token = tokenService.CreateToken(user)
            };
        }

    }
}
