using Authentication.Core.Entities;
using Authentication.Core.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Infrastructure.Repository
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _Key;
        public TokenService(IConfiguration config)
        {
            _config = config;
            var tokenKey = _config["Token:key"];
            if (string.IsNullOrEmpty(tokenKey))
            {
                throw new ArgumentNullException(nameof(tokenKey), "Token key cannot be null or empty.");
            }
            _Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        }

        public string CreateToken(AppUser appUsers)
        {
            var creds = new SigningCredentials(_Key, SecurityAlgorithms.HmacSha256Signature);
            var Claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Email, appUsers.Email ?? string.Empty),
                    new Claim(JwtRegisteredClaimNames.GivenName, appUsers.DisplayName ?? string.Empty)
                };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Claims),
                Expires = DateTime.UtcNow.AddDays(10),
                Issuer = _config["Token:Issuer"],
                SigningCredentials = creds,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
