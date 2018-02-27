using dotnet_cpnucleo_pages.Repository2;
using dotnet_cpnucleo_pages.Repository2.Projeto;
using dotnet_cpnucleo_pages.Repository2.Sistema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Pages.Projeto
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IRepository<ProjetoItem> _projetoRepository;

        private readonly IRepository<SistemaItem> _sistemaRepository;

        public IncluirModel(IRepository<ProjetoItem> projetoRepository, IRepository<SistemaItem> sistemaRepository)
        {
            _projetoRepository = projetoRepository;
            _sistemaRepository = sistemaRepository;
        }

        [BindProperty]
        public ProjetoItem Projeto { get; set; }

        public SelectList SelectSistemas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            SelectSistemas = new SelectList(await _sistemaRepository.Listar(), "IdSistema", "Nome");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(ProjetoItem projeto)
        {
            if (!ModelState.IsValid)
            {
                SelectSistemas = new SelectList(await _sistemaRepository.Listar(), "IdSistema", "Nome");

                return Page();
            }

            await _projetoRepository.Incluir(projeto);

            return RedirectToPage("Listar");
        }
    }
}