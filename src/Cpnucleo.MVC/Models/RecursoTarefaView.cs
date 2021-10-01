namespace Cpnucleo.MVC.Models;

public class RecursoTarefaView
{
    public RecursoTarefaViewModel RecursoTarefa { get; set; }

    public IEnumerable<RecursoTarefaViewModel> Lista { get; set; }

    public TarefaViewModel Tarefa { get; set; }

    public SelectList SelectRecursos { get; set; }
}
