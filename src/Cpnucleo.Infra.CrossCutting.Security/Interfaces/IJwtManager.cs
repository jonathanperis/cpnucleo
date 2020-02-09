namespace Cpnucleo.Infra.CrossCutting.Security.Interfaces
{
    public interface IJwtManager
    {
        string GenerateToken(string usuario, int tempoExpiracao);
    }
}
