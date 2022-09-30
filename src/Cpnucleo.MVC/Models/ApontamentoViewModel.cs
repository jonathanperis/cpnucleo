namespace Cpnucleo.MVC.Models;

public sealed record ApontamentoViewModel
{
    public ApontamentoDTO Apontamento { get; set; }

    public IEnumerable<ApontamentoDTO> Lista { get; set; }

    public IEnumerable<TarefaDTO> ListaTarefas { get; set; }

    public IEnumerable<WorkflowDTO> ListaWorkflow { get; set; }
}
