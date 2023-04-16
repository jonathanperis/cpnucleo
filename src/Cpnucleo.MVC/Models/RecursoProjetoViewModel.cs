namespace Cpnucleo.MVC.Models;

public sealed record RecursoProjetoViewModel
{
    public RecursoProjetoDto RecursoProjeto { get; set; }

    public IEnumerable<RecursoProjetoDto> Lista { get; set; }

    public ProjetoDto Projeto { get; set; }

    public SelectList SelectRecursos { get; set; }
}
