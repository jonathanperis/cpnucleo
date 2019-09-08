using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
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

        private readonly ITarefaAppService _tarefaAppService;

        public RemoverModel(IRecursoTarefaAppService recursoTarefaAppService,
                                       ITarefaAppService tarefaAppService)
        {
            _recursoTarefaAppService = recursoTarefaAppService;
            _tarefaAppService = tarefaAppService;
        }

        [BindProperty]
        public RecursoTarefaViewModel RecursoTarefa { get; set; }

        public IActionResult OnGet(Guid idRecursoTarefa)
        {
            RecursoTarefa = _recursoTarefaAppService.Consultar(idRecursoTarefa);

            return Page();
        }

        public IActionResult OnPost()
        {
            _recursoTarefaAppService.Remover(RecursoTarefa.Id);

            return RedirectToPage("Listar", new { idTarefa = RecursoTarefa.IdTarefa });
        }
    }
}