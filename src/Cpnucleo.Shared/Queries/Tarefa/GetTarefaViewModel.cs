﻿namespace Cpnucleo.Shared.Queries.Tarefa;

public record GetTarefaViewModel : BaseQuery
{
    public TarefaDTO Tarefa { get; set; }
    public OperationResult OperationResult { get; set; }
}
