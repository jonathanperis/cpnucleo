﻿namespace Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa;

public class GetImpedimentoTarefaByTarefaViewModel : BaseQuery
{
    public IEnumerable<ImpedimentoTarefaDTO> ImpedimentoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
