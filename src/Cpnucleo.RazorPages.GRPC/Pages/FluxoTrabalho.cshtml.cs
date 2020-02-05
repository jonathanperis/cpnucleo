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
        private readonly IWorkflowGrpcService _workflowGrpcService;
        private readonly ITarefaGrpcService _tarefaGrpcService;

        public FluxoTrabalhoModel(IWorkflowGrpcService workflowGrpcService,
                                        ITarefaGrpcService tarefaGrpcService)
        {
            _workflowGrpcService = workflowGrpcService;
            _tarefaGrpcService = tarefaGrpcService;
        }

        public IEnumerable<WorkflowViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Lista = await _workflowGrpcService.ListarPorTarefaAsync();

            return Page();
        }

        public async Task<IActionResult> OnPost(Guid idTarefa, Guid idWorkflow)
        {
            await _tarefaGrpcService.AlterarPorWorkflowAsync(idTarefa, idWorkflow);

            return Page();
        }
    }
}