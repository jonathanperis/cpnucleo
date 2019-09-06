using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.Tarefa
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly ITarefaRepository _tarefaRepository;

        public RemoverModel(ITarefaRepository tarefaRepository) => _tarefaRepository = tarefaRepository;

        [BindProperty]
        public TarefaModel Tarefa { get; set; }

        public async Task<IActionResult> OnGetAsync(int idTarefa)
        {
            Tarefa = await _tarefaRepository.ConsultarAsync(idTarefa);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _tarefaRepository.RemoverAsync(Tarefa);

            return RedirectToPage("Listar");
        }
    }
}