using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_cpnucleo_pages.Repository.Recurso;
using Microsoft.AspNetCore.Authorization;

namespace dotnet_cpnucleo_pages.Pages.Recurso
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IRecursoRepository _recursoRepository;

        public IncluirModel(IRecursoRepository recursoRepository)
        {
            _recursoRepository = recursoRepository;
        }

        [BindProperty]
        public RecursoItem Recurso { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(RecursoItem recurso)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _recursoRepository.Incluir(recurso);

            return RedirectToPage("Listar");
        }
    }
}