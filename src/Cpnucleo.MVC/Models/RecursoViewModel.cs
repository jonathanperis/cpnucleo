using Cpnucleo.Shared.Common.DTOs;

namespace Cpnucleo.MVC.Models;

public class RecursoViewModel
{
    public RecursoDTO Recurso { get; set; }

    public IEnumerable<RecursoDTO> Lista { get; set; }
}
