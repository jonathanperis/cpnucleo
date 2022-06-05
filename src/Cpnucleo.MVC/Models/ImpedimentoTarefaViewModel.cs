namespace Cpnucleo.MVC.Models;

public class ImpedimentoTarefaViewModel
{
    public ImpedimentoTarefaDTO ImpedimentoTarefa { get; set; }

    public IEnumerable<ImpedimentoTarefaDTO> Lista { get; set; }

    public TarefaDTO Tarefa { get; set; }

    public SelectList SelectImpedimentos { get; set; }
}
