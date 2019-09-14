using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Pages.Impedimento
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IAppService<ImpedimentoViewModel> _impedimentoAppService;

        public RemoverModel(IAppService<ImpedimentoViewModel> impedimentoAppService)
        {
            _impedimentoAppService = impedimentoAppService;
        }

        [BindProperty]
        public ImpedimentoViewModel Impedimento { get; set; }

        public IActionResult OnGet(Guid idImpedimento)
        {
            Impedimento = _impedimentoAppService.Consultar(idImpedimento);

            return Page();
        }

        public IActionResult OnPost()
        {
            _impedimentoAppService.Remover(Impedimento.Id);

            return RedirectToPage("Listar");
        }
    }
}