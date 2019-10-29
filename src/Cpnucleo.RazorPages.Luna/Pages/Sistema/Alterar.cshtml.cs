using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Luna.Pages.Sistema
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly ISistemaAppService _sistemaAppService;

        public AlterarModel(ISistemaAppService sistemaAppService)
        {
            _sistemaAppService = sistemaAppService;
        }

        [BindProperty]
        public SistemaViewModel Sistema { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Sistema = _sistemaAppService.Consultar(id);

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _sistemaAppService.Alterar(Sistema);

            return RedirectToPage("Listar");
        }
    }
}