using Cpnucleo.Shared.Common.DTOs;

namespace Cpnucleo.MVC.Models;

public class ImpedimentoViewModel
{
    public ImpedimentoDTO Impedimento { get; set; }

    public IEnumerable<ImpedimentoDTO> Lista { get; set; }
}
