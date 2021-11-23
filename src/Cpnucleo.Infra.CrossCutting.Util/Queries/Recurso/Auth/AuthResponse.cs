namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.Auth;

public class AuthResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public RecursoViewModel Recurso { get; set; }
}