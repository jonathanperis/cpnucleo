using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Cpnucleo.RazorPages.Pages.Tarefa
{
    [Authorize]
    public class AlterarModel : PageBase
    {
        private readonly ITarefaApiService _tarefaApiService;
        private readonly ICrudApiService<ProjetoViewModel> _projetoApiService;
        private readonly ICrudApiService<SistemaViewModel> _sistemaApiService;
        private readonly IWorkflowApiService _workflowApiService;
        private readonly ICrudApiService<TipoTarefaViewModel> _tipoTarefaApiService;

        public AlterarModel(IClaimsManager claimsManager,
                                    ITarefaApiService tarefaApiService,
                                    ICrudApiService<ProjetoViewModel> projetoApiService,
                                    ICrudApiService<SistemaViewModel> sistemaApiService,
                                    IWorkflowApiService workflowApiService,
                                    ICrudApiService<TipoTarefaViewModel> tipoTarefaApiService)
            : base(claimsManager)
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

        public IActionResult OnGet(Guid id)
        {
            Tarefa = _tarefaApiService.Consultar(Token, id);
            SelectProjetos = new SelectList(_projetoApiService.Listar(Token), "Id", "Nome");
            SelectSistemas = new SelectList(_sistemaApiService.Listar(Token), "Id", "Descricao");
            SelectWorkflows = new SelectList(_workflowApiService.Listar(Token), "Id", "Nome");
            SelectTipoTarefas = new SelectList(_tipoTarefaApiService.Listar(Token), "Id", "Nome");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                SelectProjetos = new SelectList(_projetoApiService.Listar(Token), "Id", "Nome");
                SelectSistemas = new SelectList(_sistemaApiService.Listar(Token), "Id", "Descricao");
                SelectWorkflows = new SelectList(_workflowApiService.Listar(Token), "Id", "Nome");
                SelectTipoTarefas = new SelectList(_tipoTarefaApiService.Listar(Token), "Id", "Nome");

                return Page();
            }

            _tarefaApiService.Alterar(Token, Tarefa);

            return RedirectToPage("Listar");
        }
    }
}