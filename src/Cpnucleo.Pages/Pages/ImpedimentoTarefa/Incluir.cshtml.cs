using Cpnucleo.Pages.Repository;
using Cpnucleo.Pages.Repository.Impedimento;
using Cpnucleo.Pages.Repository.ImpedimentoTarefa;
using Cpnucleo.Pages.Repository.Tarefa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IImpedimentoTarefaRepository _impedimentoTarefaRepository;

        private readonly IRepository<ImpedimentoItem> _impedimentoRepository;

        private readonly ITarefaRepository _tarefaRepository;

        public IncluirModel(IImpedimentoTarefaRepository impedimentoTarefaRepository,
                            IRepository<ImpedimentoItem> impedimentoRepository,
                            ITarefaRepository tarefaRepository)
        {
            _impedimentoTarefaRepository = impedimentoTarefaRepository;
            _impedimentoRepository = impedimentoRepository;
            _tarefaRepository = tarefaRepository;
        }

        [BindProperty]
        public ImpedimentoTarefaItem ImpedimentoTarefa { get; set; }

        public TarefaItem Tarefa { get; set; }

        public SelectList SelectImpedimentos { get; set; }

        public async Task<IActionResult> OnGetAsync(int idTarefa)
        {
            Tarefa = await _tarefaRepository.Consultar(idTarefa);

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

            await _impedimentoTarefaRepository.Incluir(impedimentoTarefa);

            return RedirectToPage("Listar", new { idTarefa = impedimentoTarefa.IdTarefa });
        }
    }
}