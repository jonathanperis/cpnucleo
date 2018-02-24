using System.Threading.Tasks;
using dotnet_cpnucleo_pages.Repository.Tarefa;
using Microsoft.AspNetCore.Mvc;
using dotnet_cpnucleo_pages.Repository.Sistema;
using Microsoft.AspNetCore.Mvc.Rendering;
using dotnet_cpnucleo_pages.Repository.Projeto;
using dotnet_cpnucleo_pages.Repository.Workflow;
using dotnet_cpnucleo_pages.Repository.TipoTarefa;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using dotnet_cpnucleo_pages.Repository;

namespace dotnet_cpnucleo_pages.Pages.Tarefa
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly ITarefaRepository _tarefaRepository;

        private readonly IRepository<ProjetoItem> _projetoRepository;

        private readonly IRepository<SistemaItem> _sistemaRepository;

        private readonly IWorkflowRepository _workflowRepository;

        private readonly IRepository<TipoTarefaItem> _tipoTarefaRepository;

        public AlterarModel(ITarefaRepository tarefaRepository,
                                IRepository<ProjetoItem> projetoRepository,
                                IRepository<SistemaItem> sistemaRepository,
                                IWorkflowRepository workflowRepository,
                                IRepository<TipoTarefaItem> tipoTarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
            _projetoRepository = projetoRepository;
            _sistemaRepository = sistemaRepository;
            _workflowRepository = workflowRepository;
            _tipoTarefaRepository = tipoTarefaRepository;
        }

        [BindProperty]
        public TarefaItem Tarefa { get; set; }

        public SelectList SelectProjetos { get; set; }

        public SelectList SelectSistemas { get; set; }

        public SelectList SelectWorkflows { get; set; }

        public SelectList SelectTipoTarefas { get; set; }

        public async Task<IActionResult> OnGetAsync(int idTarefa)
        {
            Tarefa = await _tarefaRepository.Consultar(idTarefa);
            SelectProjetos = new SelectList(await _projetoRepository.Listar(), "IdProjeto", "Nome");
            SelectSistemas = new SelectList(await _sistemaRepository.Listar(), "IdSistema", "Descricao");
            SelectWorkflows = new SelectList(await _workflowRepository.Listar(), "IdWorkflow", "Nome");
            SelectTipoTarefas = new SelectList(await _tipoTarefaRepository.Listar(), "IdTipoTarefa", "Nome");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(TarefaItem tarefa)
        {
            if (!ModelState.IsValid)
            {
                SelectProjetos = new SelectList(await _projetoRepository.Listar(), "IdProjeto", "Nome");
                SelectSistemas = new SelectList(await _sistemaRepository.Listar(), "IdSistema", "Descricao");
                SelectWorkflows = new SelectList(await _workflowRepository.Listar(), "IdWorkflow", "Nome");
                SelectTipoTarefas = new SelectList(await _tipoTarefaRepository.Listar(), "IdTipoTarefa", "Nome");

                return Page();
            }

            await _tarefaRepository.Alterar(tarefa);

            return RedirectToPage("Listar");
        }
    }
}