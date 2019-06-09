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

        [BindProperty(SupportsGet = true)]
        public ImpedimentoTarefaItem ImpedimentoTarefa { get; set; }

        public TarefaItem Tarefa { get; set; }

        public SelectList SelectImpedimentos { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Tarefa = await _tarefaRepository.ConsultarAsync(ImpedimentoTarefa.IdTarefa);

            SelectImpedimentos = new SelectList(await _impedimentoRepository.ListarAsync(), "IdImpedimento", "Nome");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Tarefa = await _tarefaRepository.ConsultarAsync(ImpedimentoTarefa.IdTarefa);

                SelectImpedimentos = new SelectList(await _impedimentoRepository.ListarAsync(), "IdImpedimento", "Nome");

                return Page();
            }

            await _impedimentoTarefaRepository.IncluirAsync(ImpedimentoTarefa);

            return RedirectToPage("Listar", new { idTarefa = ImpedimentoTarefa.IdTarefa });
        }
    }
}