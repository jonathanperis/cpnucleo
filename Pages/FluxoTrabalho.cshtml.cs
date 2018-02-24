using dotnet_cpnucleo_pages.Repository.Tarefa;
using dotnet_cpnucleo_pages.Repository.Workflow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Pages
{
    [Authorize]
    public class FluxoTrabalhoModel : PageModel
    {
        private readonly IWorkflowRepository _WorkflowRepository;

        private readonly ITarefaRepository _TarefaRepository;

        public FluxoTrabalhoModel(IWorkflowRepository WorkflowRepository,
                                  ITarefaRepository TarefaRepository)
        {
            _WorkflowRepository = WorkflowRepository;
            _TarefaRepository = TarefaRepository;
        }

        [BindProperty]
        public WorkflowItem Workflow { get; set; }

        [BindProperty]
        public IList<WorkflowItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _WorkflowRepository.ListarTarefasWorkflow();

            int qtdLista = Lista.Count;
            qtdLista = qtdLista == 1 ? 2 : qtdLista;

            int i = 12 / qtdLista;
            ViewData["col-lg-*"] = i;

            return Page();
        }

        public IActionResult OnPost(int idTarefa, int idWorkflow)
        {
            _TarefaRepository.AlterarPorFluxoTrabalho(idTarefa, idWorkflow);

            return Page();
        }
    }
}