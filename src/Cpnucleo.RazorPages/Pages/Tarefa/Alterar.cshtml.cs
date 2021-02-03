using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
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
        private readonly ITarefaService _tarefaService;
        private readonly ICrudService<ProjetoViewModel> _projetoService;
        private readonly ICrudService<SistemaViewModel> _sistemaService;
        private readonly ICrudService<WorkflowViewModel> _workflowService;
        private readonly ICrudService<TipoTarefaViewModel> _tipoTarefaService;

        public AlterarModel(ITarefaService tarefaService,
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

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                Tarefa = await _tarefaService.ConsultarAsync(Token, id);
                SelectProjetos = new SelectList(await _projetoService.ListarAsync(Token), "Id", "Nome");
                SelectSistemas = new SelectList(await _sistemaService.ListarAsync(Token), "Id", "Descricao");
                SelectWorkflows = new SelectList(await _workflowService.ListarAsync(Token), "Id", "Nome");
                SelectTipoTarefas = new SelectList(await _tipoTarefaService.ListarAsync(Token), "Id", "Nome");

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
                    SelectProjetos = new SelectList(await _projetoService.ListarAsync(Token), "Id", "Nome");
                    SelectSistemas = new SelectList(await _sistemaService.ListarAsync(Token), "Id", "Descricao");
                    SelectWorkflows = new SelectList(await _workflowService.ListarAsync(Token), "Id", "Nome");
                    SelectTipoTarefas = new SelectList(await _tipoTarefaService.ListarAsync(Token), "Id", "Nome");

                    return Page();
                }

                await _tarefaService.AlterarAsync(Token, Tarefa);

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