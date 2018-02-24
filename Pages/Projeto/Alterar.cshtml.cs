using System.Threading.Tasks;
using dotnet_cpnucleo_pages.Repository.Projeto;
using Microsoft.AspNetCore.Mvc;
using dotnet_cpnucleo_pages.Repository.Sistema;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using dotnet_cpnucleo_pages.Repository;

namespace dotnet_cpnucleo_pages.Pages.Projeto
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly IRepository<ProjetoItem> _projetoRepository;

        private readonly IRepository<SistemaItem> _sistemaRepository;

        public AlterarModel(IRepository<ProjetoItem> projetoRepository, IRepository<SistemaItem> sistemaRepository)
        {
            _projetoRepository = projetoRepository;
            _sistemaRepository = sistemaRepository;
        }

        [BindProperty]
        public ProjetoItem Projeto { get; set; }

        public SelectList SelectSistemas { get; set; }

        public async Task<IActionResult> OnGetAsync(int idProjeto)
        {
            Projeto = await _projetoRepository.Consultar(idProjeto);
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

            await _projetoRepository.Alterar(projeto);

            return RedirectToPage("Listar");
        }
    }
}