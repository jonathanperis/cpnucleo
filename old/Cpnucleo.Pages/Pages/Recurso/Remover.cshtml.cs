using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.Recurso
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IRecursoRepository _recursoRepository;

        public RemoverModel(IRecursoRepository recursoRepository) => _recursoRepository = recursoRepository;

        [BindProperty]
        public RecursoModel Recurso { get; set; }

        public async Task<IActionResult> OnGetAsync(int idRecurso)
        {
            Recurso = await _recursoRepository.ConsultarAsync(idRecurso);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _recursoRepository.RemoverAsync(Recurso);

            return RedirectToPage("Listar");
        }
    }
}