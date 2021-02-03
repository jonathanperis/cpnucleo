using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
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

        public FluxoTrabalhoModel(ICrudApiService<WorkflowViewModel> workflowApiService,
                                  ITarefaApiService tarefaApiService)
        {
            _workflowApiService = workflowApiService;
            _tarefaApiService = tarefaApiService;
        }

        public IEnumerable<WorkflowViewModel> Lista { get; set; }

        public IEnumerable<TarefaViewModel> ListaTarefas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Lista = await _workflowApiService.ListarAsync(Token);
                ListaTarefas = await _tarefaApiService.ListarAsync(Token, true);

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid idTarefa, Guid idWorkflow)
        {
            try
            {
                await _tarefaApiService.AlterarPorWorkflowAsync(Token, idTarefa, idWorkflow);

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}