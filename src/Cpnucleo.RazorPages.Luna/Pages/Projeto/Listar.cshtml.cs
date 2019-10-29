using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.Luna.Pages.Projeto
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IProjetoAppService _projetoAppService;

        public ListarModel(IProjetoAppService projetoAppService)
        {
            _projetoAppService = projetoAppService;
        }

        public ProjetoViewModel Projeto { get; set; }

        public IEnumerable<ProjetoViewModel> Lista { get; set; }

        public IActionResult OnGet()
        {
            Lista = _projetoAppService.Listar();

            return Page();
        }
    }
}