using dotnet_cpnucleo_pages.Repository;
using dotnet_cpnucleo_pages.Repository.Impedimento;
using dotnet_cpnucleo_pages.Repository.ImpedimentoTarefa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Pages.ImpedimentoTarefa
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

        [BindProperty]
        public ImpedimentoTarefaItem ImpedimentoTarefa { get; set; }

        public SelectList SelectImpedimentos { get; set; }

        public async Task<IActionResult> OnGetAsync(int idImpedimentoTarefa)
        {
            ImpedimentoTarefa = await _impedimentoTarefaRepository.Consultar(idImpedimentoTarefa);
            SelectImpedimentos = new SelectList(await _impedimentoRepository.Listar(), "IdImpedimento", "Nome");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(ImpedimentoTarefaItem impedimentoTarefa)
        {
            if (!ModelState.IsValid)
            {
                SelectImpedimentos = new SelectList(await _impedimentoRepository.Listar(), "IdImpedimento", "Nome");

                return Page();
            }

            await _impedimentoTarefaRepository.Alterar(impedimentoTarefa);

            return RedirectToPage("Listar", new { idTarefa = impedimentoTarefa.IdTarefa });
        }
    }
}