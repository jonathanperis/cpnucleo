namespace Cpnucleo.Shared.Queries.AuthUser;

public sealed record AuthUserViewModel : BaseQuery
{
    public string? Token { get; set; }
    public RecursoDTO? Recurso { get; set; }
    public OperationResult Status { get; set; }
}
