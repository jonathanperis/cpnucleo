using Cpnucleo.Pages.Authentication;
using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.Tarefa
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly ITarefaRepository _tarefaRepository;

        private readonly IRepository<ProjetoItem> _projetoRepository;

        private readonly IRepository<SistemaItem> _sistemaRepository;

        private readonly IWorkflowRepository _workflowRepository;

        private readonly IRepository<TipoTarefaItem> _tipoTarefaRepository;

        public IncluirModel(ITarefaRepository tarefaRepository,
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
            string retorno = ClaimsManager.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
            int idRecurso = int.Parse(retorno);

            Tarefa.IdRecurso = idRecurso;

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