using dotnet_cpnucleo_pages.Repository.Recurso;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Pages.Recurso
{
    [Authorize]    
    public class ListarModel : PageModel
    {
        private readonly IRecursoRepository _recursoRepository;

        public ListarModel(IRecursoRepository recursoRepository) => _recursoRepository = recursoRepository;

        [BindProperty]
        public RecursoItem Recurso { get; set; }

        [BindProperty]
        public IEnumerable<RecursoItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _recursoRepository.Listar();

            return Page();
        }
    }
}