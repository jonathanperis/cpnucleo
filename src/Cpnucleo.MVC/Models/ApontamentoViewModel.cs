namespace Cpnucleo.MVC.Models;

public sealed record ApontamentoViewModel
{
    public ApontamentoDto Apontamento { get; set; }

    public IEnumerable<ApontamentoDto> Lista { get; set; }

    public IEnumerable<TarefaDto> ListaTarefas { get; set; }

    public IEnumerable<WorkflowDto> ListaWorkflow { get; set; }
}
