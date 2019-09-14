using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Pages.Apontamento
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

        public IActionResult OnGet(Guid idApontamento)
        {
            Apontamento = _apontamentoAppService.Consultar(idApontamento);

            return Page();
        }

        public IActionResult OnPost()
        {
            _apontamentoAppService.Remover(Apontamento.Id);

            return RedirectToPage("Listar");
        }
    }
}