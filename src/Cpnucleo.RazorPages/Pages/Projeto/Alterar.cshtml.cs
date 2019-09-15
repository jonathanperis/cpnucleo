using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Cpnucleo.RazorPages.Pages.Projeto
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly IAppService<ProjetoViewModel> _projetoAppService;
        private readonly IAppService<SistemaViewModel> _sistemaAppService;

        public AlterarModel(IAppService<ProjetoViewModel> projetoAppService, IAppService<SistemaViewModel> sistemaAppService)
        {
            _projetoAppService = projetoAppService;
            _sistemaAppService = sistemaAppService;
        }

        [BindProperty]
        public ProjetoViewModel Projeto { get; set; }

        public SelectList SelectSistemas { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Projeto = _projetoAppService.Consultar(id);
            SelectSistemas = new SelectList(_sistemaAppService.Listar(), "Id", "Nome");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                SelectSistemas = new SelectList(_sistemaAppService.Listar(), "Id", "Nome");

                return Page();
            }

            _projetoAppService.Alterar(Projeto);

            return RedirectToPage("Listar");
        }
    }
}