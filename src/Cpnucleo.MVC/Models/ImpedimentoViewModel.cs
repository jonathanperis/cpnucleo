namespace Cpnucleo.MVC.Models;

public sealed record ImpedimentoViewModel
{
    public ImpedimentoDTO Impedimento { get; set; }

    public IEnumerable<ImpedimentoDTO> Lista { get; set; }
}
