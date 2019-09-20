using System.Collections.Generic;

namespace Cpnucleo.API.Utils
{
    public interface IJwtManager
    {
        string GenerateToken(string usuario, int tempoExpiracao);
    }
}
