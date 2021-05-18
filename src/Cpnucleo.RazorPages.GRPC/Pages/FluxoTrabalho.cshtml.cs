using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
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
                var result = await _workflowService.ListarAsync(Token);

                if (!result.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                    return Page();
                }

                Lista = result.response;  

                var result2 = await _tarefaService.ListarAsync(Token, true);

                if (!result2.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result2.code} - {result2.message}");
                    return Page();
                }

                ListaTarefas = result2.response;

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
                if (!ModelState.IsValid)
                {
                    var result = await _workflowService.ListarAsync(Token);

                    if (!result.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                        return Page();
                    }

                    Lista = result.response;  

                    var result2 = await _tarefaService.ListarAsync(Token, true);

                    if (!result2.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result2.code} - {result2.message}");
                        return Page();
                    }

                    ListaTarefas = result2.response;
                                        
                    return Page();
                }

                var result3 = await _tarefaService.AlterarPorWorkflowAsync(Token, idTarefa, idWorkflow);

                if (!result3.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result3.code} - {result3.message}");
                    return Page();
                }

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