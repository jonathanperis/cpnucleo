using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly IImpedimentoTarefaRepository _impedimentoTarefaRepository;

        private readonly IRepository<ImpedimentoItem> _impedimentoRepository;

        public AlterarModel(IImpedimentoTarefaRepository impedimentoTarefaRepository,
                                           IRepository<ImpedimentoItem> impedimentoRepository)
        {
            _impedimentoTarefaRepository = impedimentoTarefaRepository;
            _impedimentoRepository = impedimentoRepository;
        }

        [BindProperty(SupportsGet = true)]
        public ImpedimentoTarefaItem ImpedimentoTarefa { get; set; }

        public SelectList SelectImpedimentos { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ImpedimentoTarefa = await _impedimentoTarefaRepository.Consultar(ImpedimentoTarefa.IdImpedimentoTarefa);
            SelectImpedimentos = new SelectList(await _impedimentoRepository.Listar(), "IdImpedimento", "Nome");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                SelectImpedimentos = new SelectList(await _impedimentoRepository.Listar(), "IdImpedimento", "Nome");

                return Page();
            }

            await _impedimentoTarefaRepository.Alterar(ImpedimentoTarefa);

            return RedirectToPage("Listar", new { idTarefa = ImpedimentoTarefa.IdTarefa });
        }
    }
}