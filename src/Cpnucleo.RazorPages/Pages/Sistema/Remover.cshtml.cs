using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Pages.Sistema
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IAppService<SistemaViewModel> _sistemaAppService;

        public RemoverModel(IAppService<SistemaViewModel> sistemaAppService) => _sistemaAppService = sistemaAppService;

        [BindProperty]
        public SistemaViewModel Sistema { get; set; }

        public IActionResult OnGet(Guid idSistema)
        {
            Sistema = _sistemaAppService.Consultar(idSistema);

            return Page();
        }

        public IActionResult OnPost()
        {
            _sistemaAppService.Remover(Sistema.Id);

            return RedirectToPage("Listar");
        }
    }
}