using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Luna.Pages.Apontamento
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IApontamentoAppService _apontamentoAppService;

        public RemoverModel(IApontamentoAppService apontamentoAppService)
        {
            _apontamentoAppService = apontamentoAppService;
        }

        [BindProperty]
        public ApontamentoViewModel Apontamento { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Apontamento = _apontamentoAppService.Consultar(id);

            return Page();
        }

        public IActionResult OnPost()
        {
            _apontamentoAppService.Remover(Apontamento.Id);

            return RedirectToPage("Listar");
        }
    }
}