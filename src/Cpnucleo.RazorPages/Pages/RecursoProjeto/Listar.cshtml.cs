using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.Pages.RecursoProjeto
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IRecursoProjetoAppService _recursoProjetoAppService;

        public ListarModel(IRecursoProjetoAppService recursoProjetoAppService)
        {
            _recursoProjetoAppService = recursoProjetoAppService;
        }

        [BindProperty]
        public RecursoProjetoViewModel RecursoProjeto { get; set; }

        public IEnumerable<RecursoProjetoViewModel> Lista { get; set; }

        public IActionResult OnGet(Guid idProjeto)
        {
            Lista = _recursoProjetoAppService.ListarPorProjeto(idProjeto);

            ViewData["idProjeto"] = idProjeto;

            return Page();
        }
    }
}