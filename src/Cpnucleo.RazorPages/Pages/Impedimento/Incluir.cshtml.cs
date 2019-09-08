using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cpnucleo.RazorPages.Pages.Impedimento
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IAppService<ImpedimentoViewModel> _impedimentoAppService;

        public IncluirModel(IAppService<ImpedimentoViewModel> impedimentoAppService) => _impedimentoAppService = impedimentoAppService;

        [BindProperty]
        public ImpedimentoViewModel Impedimento { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            _impedimentoAppService.Incluir(Impedimento);

            return RedirectToPage("Listar");
        }
    }
}