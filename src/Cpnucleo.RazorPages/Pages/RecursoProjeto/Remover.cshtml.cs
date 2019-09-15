using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Pages.RecursoProjeto
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IRecursoProjetoAppService _recursoProjetoAppService;

        public RemoverModel(IRecursoProjetoAppService recursoProjetoAppService)
        {
            _recursoProjetoAppService = recursoProjetoAppService;
        }

        [BindProperty]
        public RecursoProjetoViewModel RecursoProjeto { get; set; }

        public IActionResult OnGet(Guid id)
        {
            RecursoProjeto = _recursoProjetoAppService.Consultar(id);

            return Page();
        }

        public IActionResult OnPost()
        {
            _recursoProjetoAppService.Remover(RecursoProjeto.Id);

            return RedirectToPage("Listar", new { idProjeto = RecursoProjeto.IdProjeto });
        }
    }
}