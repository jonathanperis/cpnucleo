namespace Cpnucleo.MVC.Models;

public class RecursoProjetoView
{
    public RecursoProjetoViewModel RecursoProjeto { get; set; }

    public IEnumerable<RecursoProjetoViewModel> Lista { get; set; }

    public ProjetoViewModel Projeto { get; set; }

    public SelectList SelectRecursos { get; set; }
}
