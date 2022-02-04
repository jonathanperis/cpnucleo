namespace Cpnucleo.Application.Queries.Apontamento.GetByRecurso;

public class GetByRecursoViewModel
{
    public IEnumerable<ApontamentoDTO> Apontamentos { get; set; }
    public OperationResult OperationResult { get; set; }
}
