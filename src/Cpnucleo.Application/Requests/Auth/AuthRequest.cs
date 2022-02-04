namespace Cpnucleo.Application.Requests.Auth;

public class AuthRequest : IRequest<AuthResponse>
{
    public AuthRequest(string usuario, string senha)
    {
        Usuario = usuario;
        Senha = senha;
    }

    public string Usuario { get; }
    public string Senha { get; }
}
