using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.Luna.Pages
{
    [Authorize]
    public class FluxoTrabalhoModel : PageModel
    {
        private readonly IWorkflowAppService _workflowAppService;
        private readonly ITarefaAppService _tarefaAppService;

        public FluxoTrabalhoModel(IWorkflowAppService workflowAppService,
                                  ITarefaAppService tarefaAppService)
        {
            _workflowAppService = workflowAppService;
            _tarefaAppService = tarefaAppService;
        }

        public IEnumerable<WorkflowViewModel> Lista { get; set; }

        public IActionResult OnGet()
        {
            Lista = _workflowAppService.ListarPorTarefa();

            return Page();
        }

        public IActionResult OnPost(Guid idTarefa, Guid idWorkflow)
        {
            _tarefaAppService.AlterarPorWorkflow(idTarefa, idWorkflow);

            return Page();
        }
    }
}