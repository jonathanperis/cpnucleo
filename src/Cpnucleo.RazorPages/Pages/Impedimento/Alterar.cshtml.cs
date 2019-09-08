using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Pages.Impedimento
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly IAppService<ImpedimentoViewModel> _impedimentoAppService;

        public AlterarModel(IAppService<ImpedimentoViewModel> impedimentoAppService) => _impedimentoAppService = impedimentoAppService;

        [BindProperty]
        public ImpedimentoViewModel Impedimento { get; set; }

        public IActionResult OnGet(Guid idImpedimento)
        {
            Impedimento = _impedimentoAppService.Consultar(idImpedimento);

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            _impedimentoAppService.Alterar(Impedimento);

            return RedirectToPage("Listar");
        }
    }
}