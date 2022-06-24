namespace Cpnucleo.MVC.Models;

public class ProjetoViewModel
{
    public ProjetoDTO Projeto { get; set; }

    public IEnumerable<ProjetoDTO> Lista { get; set; }

    public SelectList SelectSistemas { get; set; }
}
