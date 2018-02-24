using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_cpnucleo_pages.Repository.RecursoProjeto;
using dotnet_cpnucleo_pages.Repository.Recurso;
using Microsoft.AspNetCore.Authorization;
using dotnet_cpnucleo_pages.Repository.Projeto;
using dotnet_cpnucleo_pages.Repository;

namespace dotnet_cpnucleo_pages.Pages.RecursoProjeto
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IRecursoProjetoRepository _RecursoProjetoRepository;

        private readonly IRecursoRepository _RecursoRepository;

        private readonly IRepository<ProjetoItem> _ProjetoRepository;

        public RemoverModel(IRecursoProjetoRepository RecursoProjetoRepository,
                                        IRecursoRepository RecursoRepository,
                                        IRepository<ProjetoItem> ProjetoRepository)
        {
            _RecursoProjetoRepository = RecursoProjetoRepository;
            _RecursoRepository = RecursoRepository;
            _ProjetoRepository = ProjetoRepository;
        }

        [BindProperty]
        public RecursoProjetoItem RecursoProjeto { get; set; }

        public async Task<IActionResult> OnGetAsync(int idRecursoProjeto)
        {
            RecursoProjeto = await _RecursoProjetoRepository.Consultar(idRecursoProjeto);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(RecursoProjetoItem recursoProjeto)
        {
            if (!ModelState.IsValid)
            {
                recursoProjeto.Projeto = await _ProjetoRepository.Consultar(recursoProjeto.IdProjeto);

                return Page();
            }

            await _RecursoProjetoRepository.Remover(recursoProjeto);

            return RedirectToPage("Listar", new { idProjeto = recursoProjeto.IdProjeto });
        }
    }
}