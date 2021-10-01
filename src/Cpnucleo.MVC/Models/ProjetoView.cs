namespace Cpnucleo.MVC.Models;

public class ProjetoView
{
    public ProjetoViewModel Projeto { get; set; }

    public IEnumerable<ProjetoViewModel> Lista { get; set; }

    public SelectList SelectSistemas { get; set; }
}
