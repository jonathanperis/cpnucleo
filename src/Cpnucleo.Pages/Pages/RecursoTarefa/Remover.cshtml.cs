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

        [BindProperty]
        public RecursoTarefaModel RecursoTarefa { get; set; }

        public async Task<IActionResult> OnGetAsync(int idRecursoTarefa)
        {
            RecursoTarefa = await _recursoTarefaRepository.ConsultarAsync(idRecursoTarefa);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int idTarefa)
        {
            if (!ModelState.IsValid)
            {
                RecursoTarefa.Tarefa = await _tarefaRepository.ConsultarAsync(idTarefa);

                return Page();
            }

            await _recursoTarefaRepository.RemoverAsync(RecursoTarefa);

            return RedirectToPage("Listar", new { idTarefa });
        }
    }
}