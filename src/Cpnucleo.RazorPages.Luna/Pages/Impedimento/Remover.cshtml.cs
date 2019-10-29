using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Luna.Pages.Impedimento
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IImpedimentoAppService _impedimentoAppService;

        public RemoverModel(IImpedimentoAppService impedimentoAppService)
        {
            _impedimentoAppService = impedimentoAppService;
        }

        [BindProperty]
        public ImpedimentoViewModel Impedimento { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Impedimento = _impedimentoAppService.Consultar(id);

            return Page();
        }

        public IActionResult OnPost()
        {
            _impedimentoAppService.Remover(Impedimento.Id);

            return RedirectToPage("Listar");
        }
    }
}