namespace Cpnucleo.MVC.Models;

public sealed record SistemaViewModel
{
    public SistemaDto Sistema { get; set; }

    public IEnumerable<SistemaDto> Lista { get; set; }
}
