using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IImpedimentoTarefaRepository _impedimentoTarefaRepository;

        public RemoverModel(IImpedimentoTarefaRepository impedimentoTarefaRepository) => _impedimentoTarefaRepository = impedimentoTarefaRepository;

        [BindProperty]
        public ImpedimentoTarefaModel ImpedimentoTarefa { get; set; }

        public async Task<IActionResult> OnGetAsync(int idImpedimentoTarefa)
        {
            ImpedimentoTarefa = await _impedimentoTarefaRepository.ConsultarAsync(idImpedimentoTarefa);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int idTarefa)
        {
            await _impedimentoTarefaRepository.RemoverAsync(ImpedimentoTarefa);

            return RedirectToPage("Listar", new { idTarefa });
        }
    }
}