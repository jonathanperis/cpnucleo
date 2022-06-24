using Cpnucleo.Shared.Common.DTOs;

namespace Cpnucleo.MVC.Models;

public class SistemaViewModel
{
    public SistemaDTO Sistema { get; set; }

    public IEnumerable<SistemaDTO> Lista { get; set; }
}
