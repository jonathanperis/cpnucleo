using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cpnucleo.RazorPages.Material.Pages.Sistema
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly ISistemaAppService _sistemaAppService;

        public IncluirModel(ISistemaAppService sistemaAppService)
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