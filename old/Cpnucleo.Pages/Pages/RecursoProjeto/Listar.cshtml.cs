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

        [BindProperty]
        public RecursoProjetoModel RecursoProjeto { get; set; }

        public IEnumerable<RecursoProjetoModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync(int idProjeto)
        {
            Lista = await _recursoProjetoRepository.ListarPoridProjetoAsync(idProjeto);

            ViewData["idProjeto"] = idProjeto;

            return Page();
        }
    }
}