namespace Cpnucleo.Application.Common.Services;

internal sealed class TokenService
{
    public static string GenerateToken(string id, string key, string issuer, int expires)
    {
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.PrimarySid, id)
            }),
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddSeconds(Convert.ToInt32(expires)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature),
            Issuer = issuer,
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
