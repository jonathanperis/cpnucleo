using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.Tarefa
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IRepository<ProjetoModel> _projetoRepository;
        private readonly IRepository<SistemaModel> _sistemaRepository;
        private readonly IWorkflowRepository _workflowRepository;
        private readonly IRepository<TipoTarefaModel> _tipoTarefaRepository;

        public IncluirModel(ITarefaRepository tarefaRepository,
                                IRepository<ProjetoModel> projetoRepository,
                                IRepository<SistemaModel> sistemaRepository,
                                IWorkflowRepository workflowRepository,
                                IRepository<TipoTarefaModel> tipoTarefaRepository)
        {
            _tarefaRepository = tarefaRepository;
            _projetoRepository = projetoRepository;
            _sistemaRepository = sistemaRepository;
            _workflowRepository = workflowRepository;
            _tipoTarefaRepository = tipoTarefaRepository;
        }

        [BindProperty]
        public TarefaModel Tarefa { get; set; }

        public SelectList SelectProjetos { get; set; }

        public SelectList SelectSistemas { get; set; }

        public SelectList SelectWorkflows { get; set; }

        public SelectList SelectTipoTarefas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            SelectProjetos = new SelectList(await _projetoRepository.ListarAsync(), "IdProjeto", "Nome");
            SelectSistemas = new SelectList(await _sistemaRepository.ListarAsync(), "IdSistema", "Descricao");
            SelectWorkflows = new SelectList(await _workflowRepository.ListarAsync(), "IdWorkflow", "Nome");
            SelectTipoTarefas = new SelectList(await _tipoTarefaRepository.ListarAsync(), "IdTipoTarefa", "Nome");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                SelectProjetos = new SelectList(await _projetoRepository.ListarAsync(), "IdProjeto", "Nome");
                SelectSistemas = new SelectList(await _sistemaRepository.ListarAsync(), "IdSistema", "Descricao");
                SelectWorkflows = new SelectList(await _workflowRepository.ListarAsync(), "IdWorkflow", "Nome");
                SelectTipoTarefas = new SelectList(await _tipoTarefaRepository.ListarAsync(), "IdTipoTarefa", "Nome");

                return Page();
            }

            await _tarefaRepository.IncluirAsync(Tarefa);

            return RedirectToPage("Listar");
        }
    }
}