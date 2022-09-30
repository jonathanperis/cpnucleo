namespace Cpnucleo.MVC.Models;

public sealed record SistemaViewModel
{
    public SistemaDTO Sistema { get; set; }

    public IEnumerable<SistemaDTO> Lista { get; set; }
}
