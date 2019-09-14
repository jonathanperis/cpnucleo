using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.Pages.Sistema
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IAppService<SistemaViewModel> _sistemaAppService;

        public ListarModel(IAppService<SistemaViewModel> sistemaAppService)
        {
            _sistemaAppService = sistemaAppService;
        }

        public SistemaViewModel Sistema { get; set; }

        public IEnumerable<SistemaViewModel> Lista { get; set; }

        public IActionResult OnGet()
        {
            Lista = _sistemaAppService.Listar();

            return Page();
        }
    }
}