using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public WorkflowViewModel Workflow { get; set; }

        public IEnumerable<WorkflowViewModel> Lista { get; set; }

        public IActionResult OnGet()
        {
            Lista = _workflowAppService.ListarTarefasWorkflow();

            int qtdLista = Lista.Count();
            qtdLista = qtdLista == 1 ? 2 : qtdLista;

            int i = 12 / qtdLista;
            ViewData["col-lg-*"] = i;

            return Page();
        }

        public IActionResult OnPost(Guid idTarefa, Guid idWorkflow)
        {
            _tarefaAppService.AlterarPorFluxoTrabalho(idTarefa, idWorkflow);

            _hubContext.Clients.All.SendAsync("send", "-> Lorem Ipsum");

            return Page();
        }
    }
}