using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages
{
    [Authorize]
    public class FluxoTrabalhoModel : PageModel
    {
        private readonly ICrudGrpcService<WorkflowViewModel> _workflowGrpcService;
        private readonly ITarefaGrpcService _tarefaGrpcService;

        public FluxoTrabalhoModel(ICrudGrpcService<WorkflowViewModel> workflowGrpcService,
                                        ITarefaGrpcService tarefaGrpcService)
        {
            _workflowGrpcService = workflowGrpcService;
            _tarefaGrpcService = tarefaGrpcService;
        }

        public IEnumerable<WorkflowViewModel> Lista { get; set; }

        public IEnumerable<TarefaViewModel> ListaTarefas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _workflowGrpcService.ListarAsync();
            ListaTarefas = await _tarefaGrpcService.ListarAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid idTarefa, Guid idWorkflow)
        {
            await _tarefaGrpcService.AlterarPorWorkflowAsync(idTarefa, idWorkflow);

            return Page();
        }
    }
}