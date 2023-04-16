namespace Cpnucleo.MVC.Models;

public sealed record RecursoViewModel
{
    public RecursoDto Recurso { get; set; }

    public IEnumerable<RecursoDto> Lista { get; set; }
}
