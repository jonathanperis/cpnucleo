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
        private readonly ICrudService<WorkflowViewModel> _workflowService;
        private readonly ITarefaService _tarefaService;

        public FluxoTrabalhoModel(ICrudService<WorkflowViewModel> workflowService,
                                  ITarefaService tarefaService)
        {
            _workflowService = workflowService;
            _tarefaService = tarefaService;
        }

        public IEnumerable<WorkflowViewModel> Lista { get; set; }

        public IEnumerable<TarefaViewModel> ListaTarefas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Lista = await _workflowService.ListarAsync(Token);
                ListaTarefas = await _tarefaService.ListarAsync(Token, true);

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
                await _tarefaService.AlterarPorWorkflowAsync(Token, idTarefa, idWorkflow);

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