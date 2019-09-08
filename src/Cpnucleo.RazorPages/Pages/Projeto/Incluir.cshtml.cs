using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cpnucleo.RazorPages.Pages.Projeto
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IAppService<ProjetoViewModel> _projetoAppService;

        private readonly IAppService<SistemaViewModel> _sistemaAppService;

        public IncluirModel(IAppService<ProjetoViewModel> projetoAppService, IAppService<SistemaViewModel> sistemaAppService)
        {
            _projetoAppService = projetoAppService;
            _sistemaAppService = sistemaAppService;
        }

        [BindProperty]
        public ProjetoViewModel Projeto { get; set; }

        public SelectList SelectSistemas { get; set; }

        public IActionResult OnGet()
        {
            SelectSistemas = new SelectList(_sistemaAppService.Listar(), "IdSistema", "Nome");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                SelectSistemas = new SelectList(_sistemaAppService.Listar(), "IdSistema", "Nome");

                return Page();
            }

            _projetoAppService.Incluir(Projeto);

            return RedirectToPage("Listar");
        }
    }
}