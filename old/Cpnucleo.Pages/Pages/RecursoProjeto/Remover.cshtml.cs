using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.RecursoProjeto
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IRecursoProjetoRepository _recursoProjetoRepository;

        public RemoverModel(IRecursoProjetoRepository recursoProjetoRepository) => _recursoProjetoRepository = recursoProjetoRepository;

        [BindProperty]
        public RecursoProjetoModel RecursoProjeto { get; set; }

        public async Task<IActionResult> OnGetAsync(int idRecursoProjeto)
        {
            RecursoProjeto = await _recursoProjetoRepository.ConsultarAsync(idRecursoProjeto);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int idProjeto)
        {
            await _recursoProjetoRepository.RemoverAsync(RecursoProjeto);

            return RedirectToPage("Listar", new { idProjeto });
        }
    }
}