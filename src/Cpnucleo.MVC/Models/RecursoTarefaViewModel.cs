namespace Cpnucleo.MVC.Models;

public sealed record RecursoTarefaViewModel
{
    public RecursoTarefaDTO RecursoTarefa { get; set; }

    public IEnumerable<RecursoTarefaDTO> Lista { get; set; }

    public TarefaDTO Tarefa { get; set; }

    public SelectList SelectRecursos { get; set; }
}
