using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.RecursoProjeto
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IRecursoProjetoRepository _recursoProjetoRepository;

        private readonly IRecursoRepository _recursoRepository;

        private readonly IRepository<ProjetoItem> _projetoRepository;

        public IncluirModel(IRecursoProjetoRepository recursoProjetoRepository,
                                        IRecursoRepository recursoRepository,
                                        IRepository<ProjetoItem> projetoRepository)
        {
            _recursoProjetoRepository = recursoProjetoRepository;
            _recursoRepository = recursoRepository;
            _projetoRepository = projetoRepository;
        }

        [BindProperty(SupportsGet = true)]
        public RecursoProjetoItem RecursoProjeto { get; set; }

        public SelectList SelectRecursos { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            RecursoProjeto.Projeto = await _projetoRepository.Consultar(RecursoProjeto.IdProjeto);
            SelectRecursos = new SelectList(await _recursoRepository.Listar(), "IdRecurso", "Nome");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                RecursoProjeto.Projeto = await _projetoRepository.Consultar(RecursoProjeto.IdProjeto);
                SelectRecursos = new SelectList(await _recursoRepository.Listar(), "IdRecurso", "Nome");

                return Page();
            }

            await _recursoProjetoRepository.Incluir(RecursoProjeto);

            return RedirectToPage("Listar", new { idProjeto = RecursoProjeto.IdProjeto });
        }
    }
}