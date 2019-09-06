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

        private readonly IRepository<ProjetoModel> _projetoRepository;

        public IncluirModel(IRecursoProjetoRepository recursoProjetoRepository,
                                        IRecursoRepository recursoRepository,
                                        IRepository<ProjetoModel> projetoRepository)
        {
            _recursoProjetoRepository = recursoProjetoRepository;
            _recursoRepository = recursoRepository;
            _projetoRepository = projetoRepository;
        }

        public RecursoProjetoModel RecursoProjeto { get; set; }

        [BindProperty]
        public ProjetoModel Projeto { get; set; }

        public SelectList SelectRecursos { get; set; }

        public async Task<IActionResult> OnGetAsync(int idProjeto)
        {
            Projeto = await _projetoRepository.ConsultarAsync(idProjeto);
            SelectRecursos = new SelectList(await _recursoRepository.ListarAsync(), "IdRecurso", "Nome");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int idProjeto)
        {
            if (!ModelState.IsValid)
            {
                Projeto = await _projetoRepository.ConsultarAsync(idProjeto);
                SelectRecursos = new SelectList(await _recursoRepository.ListarAsync(), "IdRecurso", "Nome");

                return Page();
            }

            await _recursoProjetoRepository.IncluirAsync(RecursoProjeto);

            return RedirectToPage("Listar", new { idProjeto });
        }
    }
}