﻿namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto;

public class GetRecursoProjetoByProjetoViewModel : BaseQuery
{
    public IEnumerable<RecursoProjetoDTO> RecursoProjetos { get; set; }
    public OperationResult OperationResult { get; set; }
}