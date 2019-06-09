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

        [BindProperty(SupportsGet = true)]
        public RecursoProjetoItem RecursoProjeto { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            RecursoProjeto = await _recursoProjetoRepository.Consultar(RecursoProjeto.IdRecursoProjeto);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                RecursoProjeto.Projeto = await _projetoRepository.Consultar(RecursoProjeto.IdProjeto);

                return Page();
            }

            await _recursoProjetoRepository.Remover(RecursoProjeto);

            return RedirectToPage("Listar", new { idProjeto = RecursoProjeto.IdProjeto });
        }
    }
}