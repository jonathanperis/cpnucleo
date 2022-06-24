namespace Cpnucleo.MVC.Models;

public class ApontamentoViewModel
{
    public ApontamentoDTO Apontamento { get; set; }

    public IEnumerable<ApontamentoDTO> Lista { get; set; }

    public IEnumerable<TarefaDTO> ListaTarefas { get; set; }

    public IEnumerable<WorkflowDTO> ListaWorkflow { get; set; }
}
