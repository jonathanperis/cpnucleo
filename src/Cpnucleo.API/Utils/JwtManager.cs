using Cpnucleo.Infra.CrossCutting.Configuration.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Cpnucleo.API.Utils
{
    public class JwtManager : IJwtManager
    {
        private readonly ISystemConfiguration _systemConfiguration;

        public JwtManager(ISystemConfiguration systemConfiguration)
        {
            _systemConfiguration = systemConfiguration;
        }

        public string GenerateToken(string usuario, int tempoExpiracao)
        {
            var chaveSimetrica = Convert.FromBase64String(_systemConfiguration.JwtKey);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;

            List<Claim> claimsTemp = new List<Claim>();
            claimsTemp.Add(new Claim(ClaimTypes.Name, usuario));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claimsTemp),
                Expires = now.AddMinutes(Convert.ToInt32(tempoExpiracao)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chaveSimetrica), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }
    }
}
