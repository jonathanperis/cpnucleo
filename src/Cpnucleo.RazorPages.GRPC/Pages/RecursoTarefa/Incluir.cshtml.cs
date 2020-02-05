using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.RecursoTarefa
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IRecursoTarefaGrpcService _recursoTarefaGrpcService;
        private readonly IRecursoProjetoGrpcService _recursoProjetoGrpcService;
        private readonly ITarefaGrpcService _tarefaGrpcService;

        public IncluirModel(IRecursoTarefaGrpcService recursoTarefaGrpcService,
                                    IRecursoProjetoGrpcService recursoProjetoGrpcService,
                                    ITarefaGrpcService tarefaGrpcService)
        {
            _recursoTarefaGrpcService = recursoTarefaGrpcService;
            _recursoProjetoGrpcService = recursoProjetoGrpcService;
            _tarefaGrpcService = tarefaGrpcService;
        }

        [BindProperty]
        public RecursoTarefaViewModel RecursoTarefa { get; set; }

        public TarefaViewModel Tarefa { get; set; }

        public SelectList SelectRecursos { get; set; }

        public async Task<IActionResult> OnGet(Guid idTarefa)
        {
            Tarefa = await _tarefaGrpcService.ConsultarAsync(idTarefa);

            SelectRecursos = new SelectList(await _recursoProjetoGrpcService.ListarPorProjetoAsync(Tarefa.IdProjeto), "Recurso.Id", "Recurso.Nome");

            return Page();
        }

        public async Task<IActionResult> OnPost(Guid idTarefa)
        {
            if (!ModelState.IsValid)
            {
                Tarefa = await _tarefaGrpcService.ConsultarAsync(idTarefa);
                SelectRecursos = new SelectList(await _recursoProjetoGrpcService.ListarPorProjetoAsync(Tarefa.IdProjeto), "Recurso.Id", "Recurso.Nome");

                return Page();
            }

            await _recursoTarefaGrpcService.IncluirAsync(RecursoTarefa);

            return RedirectToPage("Listar", new { idTarefa });
        }
    }
}