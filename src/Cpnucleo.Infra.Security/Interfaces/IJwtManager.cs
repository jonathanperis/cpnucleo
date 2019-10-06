namespace Cpnucleo.Infra.Security.Interfaces
{
    public interface IJwtManager
    {
        string GenerateToken(string usuario, int tempoExpiracao);
    }
}
