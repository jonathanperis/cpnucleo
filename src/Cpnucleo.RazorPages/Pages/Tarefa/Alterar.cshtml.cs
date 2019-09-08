using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Cpnucleo.RazorPages.Pages.Tarefa
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly ITarefaAppService _tarefaAppService;

        private readonly IAppService<ProjetoViewModel> _projetoAppService;

        private readonly IAppService<SistemaViewModel> _sistemaAppService;

        private readonly IWorkflowAppService _workflowAppService;

        private readonly IAppService<TipoTarefaViewModel> _tipoTarefaAppService;

        public AlterarModel(ITarefaAppService tarefaAppService,
                                IAppService<ProjetoViewModel> projetoAppService,
                                IAppService<SistemaViewModel> sistemaAppService,
                                IWorkflowAppService workflowAppService,
                                IAppService<TipoTarefaViewModel> tipoTarefaAppService)
        {
            _tarefaAppService = tarefaAppService;
            _projetoAppService = projetoAppService;
            _sistemaAppService = sistemaAppService;
            _workflowAppService = workflowAppService;
            _tipoTarefaAppService = tipoTarefaAppService;
        }

        [BindProperty]
        public TarefaViewModel Tarefa { get; set; }

        public SelectList SelectProjetos { get; set; }

        public SelectList SelectSistemas { get; set; }

        public SelectList SelectWorkflows { get; set; }

        public SelectList SelectTipoTarefas { get; set; }

        public IActionResult OnGet(Guid idTarefa)
        {
            Tarefa = _tarefaAppService.Consultar(idTarefa);
            SelectProjetos = new SelectList(_projetoAppService.Listar(), "IdProjeto", "Nome");
            SelectSistemas = new SelectList(_sistemaAppService.Listar(), "IdSistema", "Descricao");
            SelectWorkflows = new SelectList(_workflowAppService.Listar(), "IdWorkflow", "Nome");
            SelectTipoTarefas = new SelectList(_tipoTarefaAppService.Listar(), "IdTipoTarefa", "Nome");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                SelectProjetos = new SelectList(_projetoAppService.Listar(), "IdProjeto", "Nome");
                SelectSistemas = new SelectList(_sistemaAppService.Listar(), "IdSistema", "Descricao");
                SelectWorkflows = new SelectList(_workflowAppService.Listar(), "IdWorkflow", "Nome");
                SelectTipoTarefas = new SelectList(_tipoTarefaAppService.Listar(), "IdTipoTarefa", "Nome");

                return Page();
            }

            _tarefaAppService.Alterar(Tarefa);

            return RedirectToPage("Listar");
        }
    }
}