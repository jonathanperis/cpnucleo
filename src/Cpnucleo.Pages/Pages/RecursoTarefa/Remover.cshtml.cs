using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.RecursoTarefa
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IRecursoTarefaRepository _recursoTarefaRepository;

        private readonly ITarefaRepository _tarefaRepository;

        public RemoverModel(IRecursoTarefaRepository recursoTarefaRepository,
                                       ITarefaRepository tarefaRepository)
        {
            _recursoTarefaRepository = recursoTarefaRepository;
            _tarefaRepository = tarefaRepository;
        }

        [BindProperty(SupportsGet = true)]
        public RecursoTarefaItem RecursoTarefa { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            RecursoTarefa = await _recursoTarefaRepository.Consultar(RecursoTarefa.IdRecursoTarefa);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                RecursoTarefa.Tarefa = await _tarefaRepository.Consultar(RecursoTarefa.IdTarefa);

                return Page();
            }

            await _recursoTarefaRepository.Remover(RecursoTarefa);

            return RedirectToPage("Listar", new { idTarefa = RecursoTarefa.IdTarefa });
        }
    }
}