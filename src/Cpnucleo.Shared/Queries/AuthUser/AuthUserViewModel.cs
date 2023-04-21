namespace Cpnucleo.Shared.Queries.AuthUser;

public sealed record AuthUserViewModel : BaseQuery
{
    public string? Token { get; set; }
    public RecursoDto? Recurso { get; set; }
    public required OperationResult OperationResult { get; set; }
}
