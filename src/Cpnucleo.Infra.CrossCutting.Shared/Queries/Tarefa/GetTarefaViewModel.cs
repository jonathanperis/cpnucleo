﻿namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Tarefa;

public class GetTarefaViewModel : BaseQuery
{
    public TarefaDTO Tarefa { get; set; }
    public OperationResult OperationResult { get; set; }
}