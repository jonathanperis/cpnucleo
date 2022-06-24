namespace Cpnucleo.MVC.Models;

public class RecursoTarefaViewModel
{
    public RecursoTarefaDTO RecursoTarefa { get; set; }

    public IEnumerable<RecursoTarefaDTO> Lista { get; set; }

    public TarefaDTO Tarefa { get; set; }

    public SelectList SelectRecursos { get; set; }
}
