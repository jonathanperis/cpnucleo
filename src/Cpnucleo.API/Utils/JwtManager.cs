using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
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
            byte[] chaveSimetrica = Convert.FromBase64String(_systemConfiguration.JwtKey);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            DateTime now = DateTime.UtcNow;

            List<Claim> claimsTemp = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario)
            };

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claimsTemp),
                Expires = now.AddMinutes(Convert.ToInt32(tempoExpiracao)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chaveSimetrica), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken stoken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(stoken);

            return token;
        }
    }
}
