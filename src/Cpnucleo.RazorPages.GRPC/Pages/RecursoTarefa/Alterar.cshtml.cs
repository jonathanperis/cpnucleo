using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.RecursoTarefa
{
    [Authorize]
    public class AlterarModel : PageBase
    {
        private readonly IRecursoTarefaApiService _recursoTarefaApiService;
        private readonly IRecursoProjetoApiService _recursoProjetoApiService;
        private readonly ITarefaApiService _tarefaApiService;

        public AlterarModel(IClaimsManager claimsManager,
                                    IRecursoTarefaApiService recursoTarefaApiService,
                                    IRecursoProjetoApiService recursoProjetoApiService,
                                    ITarefaApiService tarefaApiService)
            : base(claimsManager)
        {
            _recursoTarefaApiService = recursoTarefaApiService;
            _recursoProjetoApiService = recursoProjetoApiService;
            _tarefaApiService = tarefaApiService;
        }

        [BindProperty]
        public RecursoTarefaViewModel RecursoTarefa { get; set; }

        public TarefaViewModel Tarefa { get; set; }

        public SelectList SelectRecursos { get; set; }

        public async Task<IActionResult> OnGet(Guid id)
        {
            RecursoTarefa = _recursoTarefaApiService.Consultar(Token, id);
            SelectRecursos = new SelectList(_recursoProjetoApiService.ListarPorProjeto(Token, RecursoTarefa.Tarefa.IdProjeto), "Recurso.Id", "Recurso.Nome");

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                Tarefa = _tarefaApiService.Consultar(Token, RecursoTarefa.IdTarefa);
                SelectRecursos = new SelectList(_recursoProjetoApiService.ListarPorProjeto(Token, Tarefa.IdProjeto), "Recurso.Id", "Recurso.Nome");

                return Page();
            }

            _recursoTarefaApiService.Alterar(Token, RecursoTarefa);

            return RedirectToPage("Listar", new { idTarefa = RecursoTarefa.IdTarefa });
        }
    }
}