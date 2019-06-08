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

        private readonly IRepository<ProjetoItem> _projetoRepository;

        public RemoverModel(IRecursoProjetoRepository recursoProjetoRepository,
                                        IRepository<ProjetoItem> projetoRepository)
        {
            _recursoProjetoRepository = recursoProjetoRepository;
            _projetoRepository = projetoRepository;
        }

        [BindProperty]
        public RecursoProjetoItem RecursoProjeto { get; set; }

        public async Task<IActionResult> OnGetAsync(int idRecursoProjeto)
        {
            RecursoProjeto = await _recursoProjetoRepository.Consultar(idRecursoProjeto);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(RecursoProjetoItem recursoProjeto)
        {
            if (!ModelState.IsValid)
            {
                recursoProjeto.Projeto = await _projetoRepository.Consultar(recursoProjeto.IdProjeto);

                return Page();
            }

            await _recursoProjetoRepository.Remover(recursoProjeto);

            return RedirectToPage("Listar", new { idProjeto = recursoProjeto.IdProjeto });
        }
    }
}