namespace Cpnucleo.MVC.Models;

public sealed record ImpedimentoTarefaViewModel
{
    public ImpedimentoTarefaDto ImpedimentoTarefa { get; set; }

    public IEnumerable<ImpedimentoTarefaDto> Lista { get; set; }

    public TarefaDto Tarefa { get; set; }

    public SelectList SelectImpedimentos { get; set; }
}
