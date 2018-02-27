using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using dotnet_cpnucleo_pages.Repository.Apontamento;
using dotnet_cpnucleo_pages.Repository.RecursoTarefa;
using dotnet_cpnucleo_pages.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace dotnet_cpnucleo_pages.Pages
{
    [Authorize]
    public class ApontamentoModel : PageModel
    {
        private readonly IApontamentoRepository _apontamentoRepository;

        private readonly IRecursoTarefaRepository _recursoTarefaRepository;

        public ApontamentoModel(IApontamentoRepository apontamentoRepository,
                                     IRecursoTarefaRepository recursoTarefaRepository)
        {
            _apontamentoRepository = apontamentoRepository;
            _recursoTarefaRepository = recursoTarefaRepository;
        }

        [BindProperty]
        public ApontamentoItem Apontamento { get; set; }

        [BindProperty]
        public IEnumerable<ApontamentoItem> Lista { get; set; }

        [BindProperty]
        public IEnumerable<RecursoTarefaItem> ListaRecursoTarefas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string retorno = ClaimsManager.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
            int idRecurso = int.Parse(retorno);

            Lista = await _apontamentoRepository.ListarPoridRecurso(idRecurso);
            ListaRecursoTarefas = await _recursoTarefaRepository.ListarPoridRecurso(idRecurso);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(ApontamentoItem apontamento)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _apontamentoRepository.Incluir(apontamento);

            return RedirectToPage("Apontamento", new { idRecurso = apontamento.IdRecurso });
        }
    }
}