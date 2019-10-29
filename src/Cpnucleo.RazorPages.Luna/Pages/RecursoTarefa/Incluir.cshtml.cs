using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Cpnucleo.RazorPages.Luna.Pages.RecursoTarefa
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IRecursoTarefaAppService _recursoTarefaAppService;
        private readonly IRecursoProjetoAppService _recursoProjetoAppService;
        private readonly ITarefaAppService _tarefaAppService;

        public IncluirModel(IRecursoTarefaAppService recursoTarefaAppService,
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

        public IActionResult OnGet(Guid idTarefa)
        {
            Tarefa = _tarefaAppService.Consultar(idTarefa);

            SelectRecursos = new SelectList(_recursoProjetoAppService.ListarPorProjeto(Tarefa.IdProjeto), "Recurso.Id", "Recurso.Nome");

            return Page();
        }

        public IActionResult OnPost(Guid idTarefa)
        {
            if (!ModelState.IsValid)
            {
                Tarefa = _tarefaAppService.Consultar(idTarefa);
                SelectRecursos = new SelectList(_recursoProjetoAppService.ListarPorProjeto(Tarefa.IdProjeto), "Recurso.Id", "Recurso.Nome");

                return Page();
            }

            _recursoTarefaAppService.Incluir(RecursoTarefa);

            return RedirectToPage("Listar", new { idTarefa });
        }
    }
}