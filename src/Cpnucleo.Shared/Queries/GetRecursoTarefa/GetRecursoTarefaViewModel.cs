namespace Cpnucleo.Shared.Queries.GetRecursoTarefa;

public sealed record GetRecursoTarefaViewModel : BaseQuery
{
    public RecursoTarefaDto? RecursoTarefa { get; set; }
    public required OperationResult OperationResult { get; set; }
}
