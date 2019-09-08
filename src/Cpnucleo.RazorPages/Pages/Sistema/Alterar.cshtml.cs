using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Pages.Sistema
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly IAppService<SistemaViewModel> _sistemaAppService;

        public AlterarModel(IAppService<SistemaViewModel> sistemaAppService) => _sistemaAppService = sistemaAppService;

        [BindProperty]
        public SistemaViewModel Sistema { get; set; }

        public IActionResult OnGet(Guid idSistema)
        {
            Sistema = _sistemaAppService.Consultar(idSistema);

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            _sistemaAppService.Alterar(Sistema);

            return RedirectToPage("Listar");
        }
    }
}