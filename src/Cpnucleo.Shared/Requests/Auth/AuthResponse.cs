namespace Cpnucleo.Shared.Requests.Auth;

public sealed record AuthResponse()
{
    public string Token { get; set; }
    public RecursoDTO Recurso { get; set; }
    public OperationResult Status { get; set; }
}
