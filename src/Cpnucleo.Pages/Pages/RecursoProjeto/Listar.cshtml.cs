using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.RecursoProjeto
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IRecursoProjetoRepository _recursoProjetoRepository;

        public ListarModel(IRecursoProjetoRepository recursoProjetoRepository) => _recursoProjetoRepository = recursoProjetoRepository;

        [BindProperty(SupportsGet = true)]
        public RecursoProjetoItem RecursoProjeto { get; set; }

        public IEnumerable<RecursoProjetoItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _recursoProjetoRepository.ListarPoridProjetoAsync(RecursoProjeto.IdProjeto);

            ViewData["idProjeto"] = RecursoProjeto.IdProjeto;

            return Page();
        }
    }
}