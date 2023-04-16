namespace Cpnucleo.MVC.Models;

public sealed record ImpedimentoViewModel
{
    public ImpedimentoDto Impedimento { get; set; }

    public IEnumerable<ImpedimentoDto> Lista { get; set; }
}
