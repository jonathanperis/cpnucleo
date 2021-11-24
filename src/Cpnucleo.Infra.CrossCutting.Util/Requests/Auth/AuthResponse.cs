namespace Cpnucleo.Infra.CrossCutting.Util.Requests.Auth;

public class AuthResponse : BaseRequest
{
    public OperationResult Status { get; set; }

    public RecursoViewModel Recurso { get; set; }
}