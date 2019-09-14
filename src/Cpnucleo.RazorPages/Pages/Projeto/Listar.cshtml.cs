using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.Pages.Projeto
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IAppService<ProjetoViewModel> _projetoAppService;

        public ListarModel(IAppService<ProjetoViewModel> projetoAppService)
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