using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Pages.RecursoTarefa
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IRecursoTarefaAppService _recursoTarefaAppService;

        public RemoverModel(IRecursoTarefaAppService recursoTarefaAppService)
        {
            _recursoTarefaAppService = recursoTarefaAppService;
        }

        [BindProperty]
        public RecursoTarefaViewModel RecursoTarefa { get; set; }

        public IActionResult OnGet(Guid id)
        {
            RecursoTarefa = _recursoTarefaAppService.Consultar(id);

            return Page();
        }

        public IActionResult OnPost()
        {
            _recursoTarefaAppService.Remover(RecursoTarefa.Id);

            return RedirectToPage("Listar", new { idTarefa = RecursoTarefa.IdTarefa });
        }
    }
}