namespace Cpnucleo.MVC.Models;

public sealed record ProjetoViewModel
{
    public ProjetoDto Projeto { get; set; }

    public IEnumerable<ProjetoDto> Lista { get; set; }

    public SelectList SelectSistemas { get; set; }
}
