namespace Cpnucleo.MVC.Models;

public sealed record RecursoProjetoViewModel
{
    public RecursoProjetoDTO RecursoProjeto { get; set; }

    public IEnumerable<RecursoProjetoDTO> Lista { get; set; }

    public ProjetoDTO Projeto { get; set; }

    public SelectList SelectRecursos { get; set; }
}
