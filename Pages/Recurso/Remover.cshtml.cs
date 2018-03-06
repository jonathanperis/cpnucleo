using dotnet_cpnucleo_pages.Repository.Recurso;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Pages.Recurso
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IRecursoRepository _recursoRepository;

        public RemoverModel(IRecursoRepository recursoRepository) => _recursoRepository = recursoRepository;

        [BindProperty]
        public RecursoItem Recurso { get; set; }

        public async Task<IActionResult> OnGetAsync(int idRecurso)
        {
            Recurso = await _recursoRepository.Consultar(idRecurso);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(RecursoItem recurso)
        {
            await _recursoRepository.Remover(recurso);

            return RedirectToPage("Listar");
        }
    }
}