using dotnet_cpnucleo_pages.Repository;
using dotnet_cpnucleo_pages.Repository.Projeto;
using dotnet_cpnucleo_pages.Repository.Recurso;
using dotnet_cpnucleo_pages.Repository.RecursoProjeto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Pages.RecursoProjeto
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

        [BindProperty]
        public RecursoProjetoItem RecursoProjeto { get; set; }

        public ProjetoItem Projeto { get; set; }

        public SelectList SelectRecursos { get; set; }

        public async Task<IActionResult> OnGetAsync(int idProjeto)
        {
            Projeto = await _projetoRepository.Consultar(idProjeto);
            SelectRecursos = new SelectList(await _recursoRepository.Listar(), "IdRecurso", "Nome");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(RecursoProjetoItem recursoProjeto)
        {
            if (!ModelState.IsValid)
            {
                Projeto = await _projetoRepository.Consultar(recursoProjeto.IdProjeto);
                SelectRecursos = new SelectList(await _recursoRepository.Listar(), "IdRecurso", "Nome");

                return Page();
            }

            await _recursoProjetoRepository.Incluir(recursoProjeto);

            return RedirectToPage("Listar", new { idProjeto = recursoProjeto.IdProjeto });
        }
    }
}