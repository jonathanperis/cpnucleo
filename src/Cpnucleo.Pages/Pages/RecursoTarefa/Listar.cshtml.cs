using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
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

        [BindProperty(SupportsGet = true)]
        public RecursoTarefaItem RecursoTarefa { get; set; }

        public IEnumerable<RecursoTarefaItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _recursoTarefaRepository.ListarPoridTarefa(RecursoTarefa.IdTarefa);

            ViewData["idTarefa"] = RecursoTarefa.IdTarefa;

            return Page();
        }
    }
}