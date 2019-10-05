using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Pages.Projeto
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly ICrudAppService<ProjetoViewModel> _projetoAppService;

        public RemoverModel(ICrudAppService<ProjetoViewModel> projetoAppService)
        {
            _projetoAppService = projetoAppService;
        }

        [BindProperty]
        public ProjetoViewModel Projeto { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Projeto = _projetoAppService.Consultar(id);

            return Page();
        }

        public IActionResult OnPost()
        {
            _projetoAppService.Remover(Projeto.Id);

            return RedirectToPage("Listar");
        }
    }
}