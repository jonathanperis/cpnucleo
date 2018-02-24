using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_cpnucleo_pages.Repository.Recurso;
using Microsoft.AspNetCore.Authorization;

namespace dotnet_cpnucleo_pages.Pages.Recurso
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly IRecursoRepository _RecursoRepository;

        public AlterarModel(IRecursoRepository RecursoRepository)
        {
            _RecursoRepository = RecursoRepository;
        }

        [BindProperty]
        public RecursoItem Recurso { get; set; }

        public async Task<IActionResult> OnGetAsync(int idRecurso)
        {
            Recurso = await _RecursoRepository.Consultar(idRecurso);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(RecursoItem recurso)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _RecursoRepository.Alterar(recurso);

            return RedirectToPage("Listar");
        }
    }
}