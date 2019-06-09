using Cpnucleo.Pages.Authentication;
using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages
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

        public IEnumerable<ApontamentoItem> Lista { get; set; }

        public IEnumerable<RecursoTarefaItem> ListaRecursoTarefas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string retorno = ClaimsManager.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
            int idRecurso = int.Parse(retorno);

            Lista = await _apontamentoRepository.ListarPoridRecursoAsync(idRecurso);
            ListaRecursoTarefas = await _recursoTarefaRepository.ListarPoridRecursoAsync(idRecurso);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _apontamentoRepository.ApontarHorasAsync(Apontamento);

            return RedirectToPage("Apontamento", new { idRecurso = Apontamento.IdRecurso });
        }
    }
}