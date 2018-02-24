using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using dotnet_cpnucleo_pages.Repository.RecursoProjeto;
using dotnet_cpnucleo_pages.Repository.RecursoTarefa;
using dotnet_cpnucleo_pages.Repository.Tarefa;
using Microsoft.AspNetCore.Authorization;

namespace dotnet_cpnucleo_pages.Pages.RecursoTarefa
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly IRecursoTarefaRepository _RecursoTarefaRepository;

        private readonly IRecursoProjetoRepository _RecursoProjetoRepository;

        private readonly ITarefaRepository _TarefaRepository;

        public AlterarModel(IRecursoTarefaRepository RecursoTarefaRepository,
                                       IRecursoProjetoRepository RecursoProjetoRepository,
                                       ITarefaRepository TarefaRepository)
        {
            _RecursoTarefaRepository = RecursoTarefaRepository;
            _RecursoProjetoRepository = RecursoProjetoRepository;
            _TarefaRepository = TarefaRepository;
        }

        [BindProperty]
        public RecursoTarefaItem RecursoTarefa { get; set; }

        public SelectList SelectRecursos { get; set; }

        public async Task<IActionResult> OnGetAsync(int idRecursoTarefa)
        {
            RecursoTarefa = await _RecursoTarefaRepository.Consultar(idRecursoTarefa);
            SelectRecursos = new SelectList(await _RecursoProjetoRepository.ListarPoridProjeto(RecursoTarefa.Tarefa.IdProjeto), "Recurso.IdRecurso", "Recurso.Nome");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(RecursoTarefaItem recursoTarefa)
        {
            if (!ModelState.IsValid)
            {
                recursoTarefa.Tarefa = await _TarefaRepository.Consultar(recursoTarefa.IdTarefa);
                SelectRecursos = new SelectList(await _RecursoProjetoRepository.ListarPoridProjeto(recursoTarefa.Tarefa.IdProjeto), "Recurso.IdRecurso", "Recurso.Nome");

                return Page();
            }

            await _RecursoTarefaRepository.Alterar(recursoTarefa);

            return RedirectToPage("Listar", new { idTarefa = recursoTarefa.IdTarefa });
        }
    }
}