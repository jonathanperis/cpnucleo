using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Cpnucleo.RazorPages.Pages.RecursoTarefa
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly IRecursoTarefaAppService _recursoTarefaAppService;

        private readonly IRecursoProjetoAppService _recursoProjetoAppService;

        private readonly ITarefaAppService _tarefaAppService;

        public AlterarModel(IRecursoTarefaAppService recursoTarefaAppService,
                                       IRecursoProjetoAppService recursoProjetoAppService,
                                       ITarefaAppService tarefaAppService)
        {
            _recursoTarefaAppService = recursoTarefaAppService;
            _recursoProjetoAppService = recursoProjetoAppService;
            _tarefaAppService = tarefaAppService;
        }

        [BindProperty]
        public RecursoTarefaViewModel RecursoTarefa { get; set; }

        public TarefaViewModel Tarefa { get; set; }

        public SelectList SelectRecursos { get; set; }

        public IActionResult OnGet(Guid idRecursoTarefa)
        {
            RecursoTarefa = _recursoTarefaAppService.Consultar(idRecursoTarefa);
            SelectRecursos = new SelectList(_recursoProjetoAppService.ListarPoridProjeto(RecursoTarefa.Tarefa.IdProjeto), "Recurso.IdRecurso", "Recurso.Nome");

            return Page();
        }

        public IActionResult OnPost(Guid idTarefa)
        {
            if (!ModelState.IsValid)
            {
                Tarefa = _tarefaAppService.Consultar(idTarefa);
                SelectRecursos = new SelectList(_recursoProjetoAppService.ListarPoridProjeto(Tarefa.IdProjeto), "Recurso.IdRecurso", "Recurso.Nome");

                return Page();
            }

            _recursoTarefaAppService.Alterar(RecursoTarefa);

            return RedirectToPage("Listar", new { idTarefa });
        }
    }
}