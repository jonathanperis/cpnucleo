using dotnet_cpnucleo_pages.Repository.ImpedimentoTarefa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IImpedimentoTarefaRepository _impedimentoTarefaRepository;

        public RemoverModel(IImpedimentoTarefaRepository impedimentoTarefaRepository) => _impedimentoTarefaRepository = impedimentoTarefaRepository;

        [BindProperty]
        public ImpedimentoTarefaItem ImpedimentoTarefa { get; set; }

        public async Task<IActionResult> OnGetAsync(int idImpedimentoTarefa)
        {
            ImpedimentoTarefa = await _impedimentoTarefaRepository.Consultar(idImpedimentoTarefa);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(ImpedimentoTarefaItem impedimentoTarefa)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _impedimentoTarefaRepository.Remover(impedimentoTarefa);

            return RedirectToPage("Listar", new { idTarefa = impedimentoTarefa.IdTarefa });
        }
    }
}