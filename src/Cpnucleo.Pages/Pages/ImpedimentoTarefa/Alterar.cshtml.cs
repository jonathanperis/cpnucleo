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

        private readonly IRepository<ImpedimentoModel> _impedimentoRepository;

        public AlterarModel(IImpedimentoTarefaRepository impedimentoTarefaRepository,
                                           IRepository<ImpedimentoModel> impedimentoRepository)
        {
            _impedimentoTarefaRepository = impedimentoTarefaRepository;
            _impedimentoRepository = impedimentoRepository;
        }

        [BindProperty]
        public ImpedimentoTarefaModel ImpedimentoTarefa { get; set; }

        public SelectList SelectImpedimentos { get; set; }

        public async Task<IActionResult> OnGetAsync(int idImpedimentoTarefa)
        {
            ImpedimentoTarefa = await _impedimentoTarefaRepository.ConsultarAsync(idImpedimentoTarefa);
            SelectImpedimentos = new SelectList(await _impedimentoRepository.ListarAsync(), "IdImpedimento", "Nome");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int idTarefa)
        {
            if (!ModelState.IsValid)
            {
                SelectImpedimentos = new SelectList(await _impedimentoRepository.ListarAsync(), "IdImpedimento", "Nome");

                return Page();
            }

            await _impedimentoTarefaRepository.AlterarAsync(ImpedimentoTarefa);

            return RedirectToPage("Listar", new { idTarefa });
        }
    }
}