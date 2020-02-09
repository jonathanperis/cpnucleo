using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.Pages
{
    [Authorize]
    public class FluxoTrabalhoModel : PageBase
    {
        private readonly ICrudApiService<WorkflowViewModel> _workflowApiService;
        private readonly ITarefaApiService _tarefaApiService;

        public FluxoTrabalhoModel(IClaimsManager claimsManager,
                                        ICrudApiService<WorkflowViewModel> workflowApiService,
                                        ITarefaApiService tarefaApiService)
            : base(claimsManager)
        {
            _workflowApiService = workflowApiService;
            _tarefaApiService = tarefaApiService;
        }

        public IEnumerable<WorkflowViewModel> Lista { get; set; }

        public IEnumerable<TarefaViewModel> ListaTarefas { get; set; }

        public IActionResult OnGet()
        {
            Lista = _workflowApiService.Listar(Token);
            ListaTarefas = _tarefaApiService.Listar(Token);

            return Page();
        }

        public IActionResult OnPost(Guid idTarefa, Guid idWorkflow)
        {
            _tarefaApiService.AlterarPorWorkflow(Token, idTarefa, idWorkflow);

            return Page();
        }
    }
}