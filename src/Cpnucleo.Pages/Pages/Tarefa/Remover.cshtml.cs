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
        public TarefaItem Tarefa { get; set; }

        public async Task<IActionResult> OnGetAsync(int idTarefa)
        {
            Tarefa = await _tarefaRepository.Consultar(idTarefa);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(TarefaItem tarefa)
        {
            if (!ModelState.IsValid) return Page();

            await _tarefaRepository.Remover(tarefa);

            return RedirectToPage("Listar");
        }
    }
}