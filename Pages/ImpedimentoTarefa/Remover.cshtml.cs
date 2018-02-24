using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_cpnucleo_pages.Repository.ImpedimentoTarefa;
using dotnet_cpnucleo_pages.Repository.Tarefa;
using Microsoft.AspNetCore.Authorization;
using dotnet_cpnucleo_pages.Repository.Impedimento;
using dotnet_cpnucleo_pages.Repository;

namespace dotnet_cpnucleo_pages.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IImpedimentoTarefaRepository _ImpedimentoTarefaRepository;

        private readonly IRepository<ImpedimentoItem> _ImpedimentoRepository;

        private readonly ITarefaRepository _TarefaRepository;

        public RemoverModel(IImpedimentoTarefaRepository ImpedimentoTarefaRepository,
                            IRepository<ImpedimentoItem> ImpedimentoRepository,
                            ITarefaRepository TarefaRepository)
        {
            _ImpedimentoTarefaRepository = ImpedimentoTarefaRepository;
            _ImpedimentoRepository = ImpedimentoRepository;
            _TarefaRepository = TarefaRepository;
        }

        [BindProperty]
        public ImpedimentoTarefaItem ImpedimentoTarefa { get; set; }

        public async Task<IActionResult> OnGetAsync(int idImpedimentoTarefa)
        {
            ImpedimentoTarefa = await _ImpedimentoTarefaRepository.Consultar(idImpedimentoTarefa);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(ImpedimentoTarefaItem impedimentoTarefa)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _ImpedimentoTarefaRepository.Remover(impedimentoTarefa);

            return RedirectToPage("Listar", new { idTarefa = impedimentoTarefa.IdTarefa });
        }
    }
}