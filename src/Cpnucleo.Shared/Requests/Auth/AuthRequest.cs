namespace Cpnucleo.Shared.Requests.Auth;

public class AuthRequest : IRequest<AuthResponse>
{
    public string Usuario { get; set; }
    public string Senha { get; set; }
}
