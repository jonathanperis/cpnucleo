using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Tarefa
{
    [Authorize]
    public class IncluirModel : PageBase
    {
        private readonly ITarefaService _tarefaService;
        private readonly ICrudService<ProjetoViewModel> _projetoService;
        private readonly ICrudService<SistemaViewModel> _sistemaService;
        private readonly ICrudService<WorkflowViewModel> _workflowService;
        private readonly ICrudService<TipoTarefaViewModel> _tipoTarefaService;

        public IncluirModel(ITarefaService tarefaService,
                            ICrudService<ProjetoViewModel> projetoService,
                            ICrudService<SistemaViewModel> sistemaService,
                            ICrudService<WorkflowViewModel> workflowService,
                            ICrudService<TipoTarefaViewModel> tipoTarefaService)
        {
            _tarefaService = tarefaService;
            _projetoService = projetoService;
            _sistemaService = sistemaService;
            _workflowService = workflowService;
            _tipoTarefaService = tipoTarefaService;
        }

        [BindProperty]
        public TarefaViewModel Tarefa { get; set; }

        public SelectList SelectProjetos { get; set; }

        public SelectList SelectSistemas { get; set; }

        public SelectList SelectWorkflows { get; set; }

        public SelectList SelectTipoTarefas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var result = await _projetoService.ListarAsync(Token);

                if (!result.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                    return Page();
                }

                SelectProjetos = new SelectList(result.response, "Id", "Nome");

                var result2 = await _sistemaService.ListarAsync(Token);

                if (!result2.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result2.code} - {result2.message}");
                    return Page();
                }

                SelectSistemas = new SelectList(result2.response, "Id", "Descricao");

                var result3 = await _workflowService.ListarAsync(Token);

                if (!result3.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result3.code} - {result3.message}");
                    return Page();
                }

                SelectWorkflows = new SelectList(result3.response, "Id", "Nome");

                var result4 = await _tipoTarefaService.ListarAsync(Token);

                if (!result4.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result4.code} - {result4.message}");
                    return Page();
                }

                SelectTipoTarefas = new SelectList(result4.response, "Id", "Nome");

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var result = await _projetoService.ListarAsync(Token);

                    if (!result.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                        return Page();
                    }

                    SelectProjetos = new SelectList(result.response, "Id", "Nome");

                    var result2 = await _sistemaService.ListarAsync(Token);

                    if (!result2.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result2.code} - {result2.message}");
                        return Page();
                    }

                    SelectSistemas = new SelectList(result2.response, "Id", "Descricao");

                    var result3 = await _workflowService.ListarAsync(Token);

                    if (!result3.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result3.code} - {result3.message}");
                        return Page();
                    }

                    SelectWorkflows = new SelectList(result3.response, "Id", "Nome");

                    var result4 = await _tipoTarefaService.ListarAsync(Token);

                    if (!result4.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result4.code} - {result4.message}");
                        return Page();
                    }

                    SelectTipoTarefas = new SelectList(result4.response, "Id", "Nome");

                    return Page();
                }

                var result5 = await _tarefaService.IncluirAsync(Token, Tarefa);

                if (!result5.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result5.code} - {result5.message}");
                    return Page();
                }

                return RedirectToPage("Listar");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}