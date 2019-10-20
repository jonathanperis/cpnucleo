using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.Material.Pages.Sistema
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly ISistemaAppService _sistemaAppService;

        public ListarModel(ISistemaAppService sistemaAppService)
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