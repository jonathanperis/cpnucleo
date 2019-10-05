using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cpnucleo.RazorPages.Pages.Sistema
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly ICrudAppService<SistemaViewModel> _sistemaAppService;

        public IncluirModel(ICrudAppService<SistemaViewModel> sistemaAppService)
        {
            _sistemaAppService = sistemaAppService;
        }

        [BindProperty]
        public SistemaViewModel Sistema { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _sistemaAppService.Incluir(Sistema);

            return RedirectToPage("Listar");
        }
    }
}