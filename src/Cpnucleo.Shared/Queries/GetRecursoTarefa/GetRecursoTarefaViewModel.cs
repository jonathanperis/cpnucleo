namespace Cpnucleo.Shared.Queries.GetRecursoTarefa;

public sealed record GetRecursoTarefaViewModel : BaseQuery
{
    public RecursoTarefaDto RecursoTarefa { get; set; }
    public OperationResult OperationResult { get; set; }
}
