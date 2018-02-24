using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_cpnucleo_pages.Repository.RecursoProjeto;
using dotnet_cpnucleo_pages.Repository.RecursoTarefa;
using dotnet_cpnucleo_pages.Repository.Tarefa;
using Microsoft.AspNetCore.Authorization;

namespace dotnet_cpnucleo_pages.Pages.RecursoTarefa
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IRecursoTarefaRepository _RecursoTarefaRepository;

        private readonly IRecursoProjetoRepository _RecursoProjetoRepository;

        private readonly ITarefaRepository _TarefaRepository;

        public RemoverModel(IRecursoTarefaRepository RecursoTarefaRepository,
                                       IRecursoProjetoRepository RecursoProjetoRepository,
                                       ITarefaRepository TarefaRepository)
        {
            _RecursoTarefaRepository = RecursoTarefaRepository;
            _RecursoProjetoRepository = RecursoProjetoRepository;
            _TarefaRepository = TarefaRepository;
        }

        [BindProperty]
        public RecursoTarefaItem RecursoTarefa { get; set; }

        public async Task<IActionResult> OnGetAsync(int idRecursoTarefa)
        {
            RecursoTarefa = await _RecursoTarefaRepository.Consultar(idRecursoTarefa);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(RecursoTarefaItem recursoTarefa)
        {
            if (!ModelState.IsValid)
            {
                recursoTarefa.Tarefa = await _TarefaRepository.Consultar(recursoTarefa.IdTarefa);

                return Page();
            }

            await _RecursoTarefaRepository.Remover(recursoTarefa);

            return RedirectToPage("Listar", new { idTarefa = recursoTarefa.IdTarefa });
        }
    }
}