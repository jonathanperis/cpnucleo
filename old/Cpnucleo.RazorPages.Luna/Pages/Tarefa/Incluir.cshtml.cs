using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cpnucleo.RazorPages.Luna.Pages.Tarefa
{
    [Authorize]
    public class IncluirModel : PageBase
    {
        private readonly ITarefaApiService _tarefaApiService;
        private readonly IProjetoApiService _projetoApiService;
        private readonly ISistemaApiService _sistemaApiService;
        private readonly IWorkflowApiService _workflowApiService;
        private readonly ITipoTarefaApiService _tipoTarefaApiService;

        public IncluirModel(IClaimsManager claimsManager,
                                    ITarefaApiService tarefaApiService,
                                    IProjetoApiService projetoApiService,
                                    ISistemaApiService sistemaApiService,
                                    IWorkflowApiService workflowApiService,
                                    ITipoTarefaApiService tipoTarefaApiService)
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

        public IActionResult OnGet()
        {
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

            _tarefaApiService.Incluir(Token, Tarefa);

            return RedirectToPage("Listar");
        }
    }
}