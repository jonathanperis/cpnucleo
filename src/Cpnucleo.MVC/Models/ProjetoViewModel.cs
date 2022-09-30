namespace Cpnucleo.MVC.Models;

public sealed record ProjetoViewModel
{
    public ProjetoDTO Projeto { get; set; }

    public IEnumerable<ProjetoDTO> Lista { get; set; }

    public SelectList SelectSistemas { get; set; }
}
