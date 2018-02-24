using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using dotnet_cpnucleo_pages.Repository.RecursoProjeto;
using dotnet_cpnucleo_pages.Repository.Recurso;
using Microsoft.AspNetCore.Authorization;
using dotnet_cpnucleo_pages.Repository.Projeto;
using dotnet_cpnucleo_pages.Repository;

namespace dotnet_cpnucleo_pages.Pages.RecursoProjeto
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IRecursoProjetoRepository _RecursoProjetoRepository;

        private readonly IRecursoRepository _RecursoRepository;

        private readonly IRepository<ProjetoItem> _ProjetoRepository;

        public IncluirModel(IRecursoProjetoRepository RecursoProjetoRepository,
                                        IRecursoRepository RecursoRepository,
                                        IRepository<ProjetoItem> ProjetoRepository)
        {
            _RecursoProjetoRepository = RecursoProjetoRepository;
            _RecursoRepository = RecursoRepository;
            _ProjetoRepository = ProjetoRepository;
        }

        [BindProperty]
        public RecursoProjetoItem RecursoProjeto { get; set; }

        public ProjetoItem Projeto { get; set; }

        public SelectList SelectRecursos { get; set; }

        public async Task<IActionResult> OnGetAsync(int idProjeto)
        {
            Projeto = await _ProjetoRepository.Consultar(idProjeto);
            SelectRecursos = new SelectList(await _RecursoRepository.Listar(), "IdRecurso", "Nome");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(RecursoProjetoItem recursoProjeto)
        {
            if (!ModelState.IsValid)
            {
                Projeto = await _ProjetoRepository.Consultar(recursoProjeto.IdProjeto);
                SelectRecursos = new SelectList(await _RecursoRepository.Listar(), "IdRecurso", "Nome");

                return Page();
            }

            await _RecursoProjetoRepository.Incluir(recursoProjeto);

            return RedirectToPage("Listar", new { idProjeto = recursoProjeto.IdProjeto });
        }
    }
}