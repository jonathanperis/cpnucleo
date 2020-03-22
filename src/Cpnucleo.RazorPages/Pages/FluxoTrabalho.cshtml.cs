using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _workflowApiService.ListarAsync(Token);
            ListaTarefas = await _tarefaApiService.ListarAsync(Token);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid idTarefa, Guid idWorkflow)
        {
            await _tarefaApiService.AlterarPorWorkflowAsync(Token, idTarefa, idWorkflow);

            return Page();
        }
    }
}