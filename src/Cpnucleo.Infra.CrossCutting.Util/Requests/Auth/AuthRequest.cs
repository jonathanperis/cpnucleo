namespace Cpnucleo.Infra.CrossCutting.Util.Requests.Auth;

public class AuthRequest : BaseRequest, IRequest<AuthResponse>
{
    public AuthViewModel Auth { get; set; }
}