using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Tarefa
{
    [Authorize]
    public class AlterarModel : PageBase
    {
        private readonly ITarefaApiService _tarefaApiService;
        private readonly ICrudApiService<ProjetoViewModel> _projetoApiService;
        private readonly ICrudApiService<SistemaViewModel> _sistemaApiService;
        private readonly ICrudApiService<WorkflowViewModel> _workflowApiService;
        private readonly ICrudApiService<TipoTarefaViewModel> _tipoTarefaApiService;

        public AlterarModel(ITarefaApiService tarefaApiService,
                            ICrudApiService<ProjetoViewModel> projetoApiService,
                            ICrudApiService<SistemaViewModel> sistemaApiService,
                            ICrudApiService<WorkflowViewModel> workflowApiService,
                            ICrudApiService<TipoTarefaViewModel> tipoTarefaApiService)
        {
            _tarefaApiService = tarefaApiService;
            _projetoApiService = projetoApiService;
            _sistemaApiService = sistemaApiService;
            _workflowApiService = workflowApiService;
            _tipoTarefaApiService = tipoTarefaApiService;
        }

        [BindProperty]
        public TarefaViewModel Tarefa { get; set; }

        public SelectList SelectProjetos { get; set; }

        public SelectList SelectSistemas { get; set; }

        public SelectList SelectWorkflows { get; set; }

        public SelectList SelectTipoTarefas { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                Tarefa = await _tarefaApiService.ConsultarAsync(Token, id);
                SelectProjetos = new SelectList(await _projetoApiService.ListarAsync(Token), "Id", "Nome");
                SelectSistemas = new SelectList(await _sistemaApiService.ListarAsync(Token), "Id", "Descricao");
                SelectWorkflows = new SelectList(await _workflowApiService.ListarAsync(Token), "Id", "Nome");
                SelectTipoTarefas = new SelectList(await _tipoTarefaApiService.ListarAsync(Token), "Id", "Nome");

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
                    SelectProjetos = new SelectList(await _projetoApiService.ListarAsync(Token), "Id", "Nome");
                    SelectSistemas = new SelectList(await _sistemaApiService.ListarAsync(Token), "Id", "Descricao");
                    SelectWorkflows = new SelectList(await _workflowApiService.ListarAsync(Token), "Id", "Nome");
                    SelectTipoTarefas = new SelectList(await _tipoTarefaApiService.ListarAsync(Token), "Id", "Nome");

                    return Page();
                }

                await _tarefaApiService.AlterarAsync(Token, Tarefa);

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