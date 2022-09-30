namespace Cpnucleo.MVC.Models;

public sealed record RecursoViewModel
{
    public RecursoDTO Recurso { get; set; }

    public IEnumerable<RecursoDTO> Lista { get; set; }
}
