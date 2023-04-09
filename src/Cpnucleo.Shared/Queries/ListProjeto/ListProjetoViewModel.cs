﻿namespace Cpnucleo.Shared.Queries.ListProjeto;

public sealed record ListProjetoViewModel : BaseQuery
{
    public IEnumerable<ProjetoDTO> Projetos { get; set; }
    public OperationResult OperationResult { get; set; }
}