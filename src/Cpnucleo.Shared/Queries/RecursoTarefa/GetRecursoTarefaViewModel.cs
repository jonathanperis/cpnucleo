﻿namespace Cpnucleo.Shared.Queries.RecursoTarefa;

public record GetRecursoTarefaViewModel : BaseQuery
{
    public RecursoTarefaDTO RecursoTarefa { get; set; }
    public OperationResult OperationResult { get; set; }
}
