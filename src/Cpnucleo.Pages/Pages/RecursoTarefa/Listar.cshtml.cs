using Cpnucleo.Pages.Repository.RecursoTarefa;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.RecursoTarefa
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IRecursoTarefaRepository _recursoTarefaRepository;

        public ListarModel(IRecursoTarefaRepository recursoTarefaRepository) => _recursoTarefaRepository = recursoTarefaRepository;

        [BindProperty]
        public RecursoTarefaItem RecursoTarefa { get; set; }

        [BindProperty]
        public IEnumerable<RecursoTarefaItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync(int idTarefa)
        {
            Lista = await _recursoTarefaRepository.ListarPoridTarefa(idTarefa);

            ViewData["idTarefa"] = idTarefa;

            return Page();
        }
    }
}