using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.RecursoTarefa
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IRecursoTarefaRepository _recursoTarefaRepository;

        private readonly IRecursoProjetoRepository _recursoProjetoRepository;

        private readonly ITarefaRepository _tarefaRepository;

        public IncluirModel(IRecursoTarefaRepository recursoTarefaRepository,
                                       IRecursoProjetoRepository recursoProjetoRepository,
                                       ITarefaRepository tarefaRepository)
        {
            _recursoTarefaRepository = recursoTarefaRepository;
            _recursoProjetoRepository = recursoProjetoRepository;
            _tarefaRepository = tarefaRepository;
        }

        [BindProperty]
        public RecursoTarefaItem RecursoTarefa { get; set; }

        public TarefaItem Tarefa { get; set; }

        public SelectList SelectRecursos { get; set; }

        public async Task<IActionResult> OnGetAsync(int idTarefa)
        {
            Tarefa = await _tarefaRepository.ConsultarAsync(idTarefa);

            SelectRecursos = new SelectList(await _recursoProjetoRepository.ListarPoridProjetoAsync(Tarefa.IdProjeto), "Recurso.IdRecurso", "Recurso.Nome");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int idTarefa)
        {
            if (!ModelState.IsValid)
            {
                RecursoTarefa.Tarefa = await _tarefaRepository.ConsultarAsync(idTarefa);
                SelectRecursos = new SelectList(await _recursoProjetoRepository.ListarPoridProjetoAsync(RecursoTarefa.Tarefa.IdProjeto), "Recurso.IdRecurso", "Recurso.Nome");

                return Page();
            }

            await _recursoTarefaRepository.IncluirAsync(RecursoTarefa);

            return RedirectToPage("Listar", new { idTarefa });
        }
    }
}