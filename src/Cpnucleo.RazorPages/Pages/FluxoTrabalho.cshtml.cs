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
        private readonly IHttpService _httpService;

        public FluxoTrabalhoModel(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public IEnumerable<WorkflowViewModel> Lista { get; set; }

        public IEnumerable<TarefaViewModel> ListaTarefas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var result = await _httpService.GetAsync<IEnumerable<WorkflowViewModel>>("workflow", Token);

                if (!result.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                    return Page();
                }

                Lista = result.response;  

                var result2 = await _httpService.GetAsync<IEnumerable<TarefaViewModel>>("tarefa", Token, true);

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
                    var result = await _httpService.GetAsync<IEnumerable<WorkflowViewModel>>("workflow", Token);

                    if (!result.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                        return Page();
                    }

                    Lista = result.response;  

                    var result2 = await _httpService.GetAsync<IEnumerable<TarefaViewModel>>("tarefa", Token, true);

                    if (!result2.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result2.code} - {result2.message}");
                        return Page();
                    }

                    ListaTarefas = result2.response;
                                        
                    return Page();
                }

                var result3 = await _httpService.GetAsync<WorkflowViewModel>("workflow", Token, idWorkflow);

                if (!result3.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result3.code} - {result3.message}");
                    return Page();
                }

                var result4 = await _httpService.PutAsync<TarefaViewModel>("tarefa/putByWorkflow", Token, idTarefa, result3.response);

                if (!result4.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result4.code} - {result4.message}");
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