using HumanShop.Entities;
using HumanShop.Services.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HumanShop.Services.Implementation
{
    public class TokenService(IConfiguration configuration) : ITokenService
    {
        public string CreateToken(AppUser appUser)
        {
            var token = configuration["TokenKey"] ?? throw new Exception("Can not access token");
            if (token.Length < 64) throw new Exception("Your token key need to be longer");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token));

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, appUser.UserName)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandle = new JwtSecurityTokenHandler();
            var tokens = tokenHandle.CreateToken(tokenDescriptor);
            return tokenHandle.WriteToken(tokens);
        }
    }
}
