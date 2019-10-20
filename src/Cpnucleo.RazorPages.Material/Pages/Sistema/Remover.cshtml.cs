using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Material.Pages.Sistema
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly ISistemaAppService _sistemaAppService;

        public RemoverModel(ISistemaAppService sistemaAppService)
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
            _sistemaAppService.Remover(Sistema.Id);

            return RedirectToPage("Listar");
        }
    }
}