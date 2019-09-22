using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.Pages
{
    [Authorize]
    public class FluxoTrabalhoModel : PageModel
    {
        private readonly IWorkflowAppService _workflowAppService;
        private readonly ITarefaAppService _tarefaAppService;
        private readonly IHubContext<FluxoTrabalhoHub> _hubContext;

        public FluxoTrabalhoModel(IWorkflowAppService workflowAppService,
                                  ITarefaAppService tarefaAppService,
                                  IHubContext<FluxoTrabalhoHub> hubContext)
        {
            _workflowAppService = workflowAppService;
            _tarefaAppService = tarefaAppService;
            _hubContext = hubContext;
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

            _hubContext.Clients.All.SendAsync("send", "-> Lorem Ipsum");

            return Page();
        }
    }
}