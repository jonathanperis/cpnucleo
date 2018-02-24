using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_cpnucleo_pages.Repository.Recurso;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace dotnet_cpnucleo_pages.Pages.Recurso
{
    [Authorize]    
    public class ListarModel : PageModel
    {
        private readonly IRecursoRepository _RecursoRepository;

        public ListarModel(IRecursoRepository RecursoRepository)
        {
            _RecursoRepository = RecursoRepository;
        }

        [BindProperty]
        public RecursoItem Recurso { get; set; }

        [BindProperty]
        public IList<RecursoItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _RecursoRepository.Listar();

            return Page();
        }
    }
}