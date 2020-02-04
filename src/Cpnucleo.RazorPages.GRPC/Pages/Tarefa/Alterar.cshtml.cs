using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Tarefa
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly ITarefaGrpcService _tarefaGrpcService;
        private readonly IProjetoGrpcService _projetoGrpcService;
        private readonly ISistemaGrpcService _sistemaGrpcService;
        private readonly IWorkflowGrpcService _workflowGrpcService;
        private readonly ITipoTarefaGrpcService _tipoTarefaGrpcService;

        public AlterarModel(ITarefaGrpcService tarefaGrpcService,
                                    IProjetoGrpcService projetoGrpcService,
                                    ISistemaGrpcService sistemaGrpcService,
                                    IWorkflowGrpcService workflowGrpcService,
                                    ITipoTarefaGrpcService tipoTarefaGrpcService)
        {
            _tarefaGrpcService = tarefaGrpcService;
            _projetoGrpcService = projetoGrpcService;
            _sistemaGrpcService = sistemaGrpcService;
            _workflowGrpcService = workflowGrpcService;
            _tipoTarefaGrpcService = tipoTarefaGrpcService;
        }

        [BindProperty]
        public TarefaViewModel Tarefa { get; set; }

        public SelectList SelectProjetos { get; set; }

        public SelectList SelectSistemas { get; set; }

        public SelectList SelectWorkflows { get; set; }

        public SelectList SelectTipoTarefas { get; set; }

        public async Task<IActionResult> OnGet(Guid id)
        {
            Tarefa = await _tarefaGrpcService.ConsultarAsync(id);
            SelectProjetos = new SelectList(await _projetoGrpcService.ListarAsync(), "Id", "Nome");
            SelectSistemas = new SelectList(await _sistemaGrpcService.ListarAsync(), "Id", "Descricao");
            SelectWorkflows = new SelectList(await _workflowGrpcService.ListarAsync(), "Id", "Nome");
            SelectTipoTarefas = new SelectList(await _tipoTarefaGrpcService.ListarAsync(), "Id", "Nome");

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                SelectProjetos = new SelectList(await _projetoGrpcService.ListarAsync(), "Id", "Nome");
                SelectSistemas = new SelectList(await _sistemaGrpcService.ListarAsync(), "Id", "Descricao");
                SelectWorkflows = new SelectList(await _workflowGrpcService.ListarAsync(), "Id", "Nome");
                SelectTipoTarefas = new SelectList(await _tipoTarefaGrpcService.ListarAsync(), "Id", "Nome");

                return Page();
            }

            await _tarefaGrpcService.AlterarAsync(Tarefa);

            return RedirectToPage("Listar");
        }
    }
}