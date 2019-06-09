using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IImpedimentoTarefaRepository _impedimentoTarefaRepository;

        public ListarModel(IImpedimentoTarefaRepository impedimentoTarefaRepository) => _impedimentoTarefaRepository = impedimentoTarefaRepository;

        [BindProperty(SupportsGet = true)]
        public ImpedimentoTarefaItem ImpedimentoTarefa { get; set; }

        public IEnumerable<ImpedimentoTarefaItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _impedimentoTarefaRepository.ListarPoridTarefaAsync(ImpedimentoTarefa.IdTarefa);

            ViewData["idTarefa"] = ImpedimentoTarefa.IdTarefa;

            return Page();
        }
    }
}