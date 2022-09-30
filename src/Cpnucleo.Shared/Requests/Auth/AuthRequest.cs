namespace Cpnucleo.Shared.Requests.Auth;

public sealed record AuthRequest(string Usuario, string Senha) : IRequest<AuthResponse>;
