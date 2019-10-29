using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cpnucleo.RazorPages.Luna.Pages.Impedimento
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IImpedimentoAppService _impedimentoAppService;

        public IncluirModel(IImpedimentoAppService impedimentoAppService)
        {
            _impedimentoAppService = impedimentoAppService;
        }

        [BindProperty]
        public ImpedimentoViewModel Impedimento { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _impedimentoAppService.Incluir(Impedimento);

            return RedirectToPage("Listar");
        }
    }
}