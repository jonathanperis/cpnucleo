using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.Pages.Impedimento
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly ICrudAppService<ImpedimentoViewModel> _impedimentoAppService;

        public ListarModel(ICrudAppService<ImpedimentoViewModel> impedimentoAppService)
        {
            _impedimentoAppService = impedimentoAppService;
        }

        public ImpedimentoViewModel Impedimento { get; set; }

        public IEnumerable<ImpedimentoViewModel> Lista { get; set; }

        public IActionResult OnGet()
        {
            Lista = _impedimentoAppService.Listar();

            return Page();
        }
    }
}