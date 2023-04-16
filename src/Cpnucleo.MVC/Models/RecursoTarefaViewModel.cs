namespace Cpnucleo.MVC.Models;

public sealed record RecursoTarefaViewModel
{
    public RecursoTarefaDto RecursoTarefa { get; set; }

    public IEnumerable<RecursoTarefaDto> Lista { get; set; }

    public TarefaDto Tarefa { get; set; }

    public SelectList SelectRecursos { get; set; }
}
