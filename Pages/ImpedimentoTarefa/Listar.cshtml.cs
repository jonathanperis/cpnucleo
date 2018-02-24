using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_cpnucleo_pages.Repository.ImpedimentoTarefa;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace dotnet_cpnucleo_pages.Pages.ImpedimentoTarefa
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IImpedimentoTarefaRepository _ImpedimentoTarefaRepository;

        public ListarModel(IImpedimentoTarefaRepository ImpedimentoTarefaRepository)
        {
            _ImpedimentoTarefaRepository = ImpedimentoTarefaRepository;
        }

        [BindProperty]
        public ImpedimentoTarefaItem ImpedimentoTarefa { get; set; }

        [BindProperty]
        public IList<ImpedimentoTarefaItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync(int idTarefa)
        {
            Lista = await _ImpedimentoTarefaRepository.ListarPoridTarefa(idTarefa);

            ViewData["idTarefa"] = idTarefa;

            return Page();
        }
    }
}