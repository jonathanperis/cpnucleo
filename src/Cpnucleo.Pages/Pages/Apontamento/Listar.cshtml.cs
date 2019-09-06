using Cpnucleo.Pages.Authentication;
using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.Apontamento
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IClaimsManager _claimsManager;

        private readonly IApontamentoRepository _apontamentoRepository;
        private readonly IRecursoTarefaRepository _recursoTarefaRepository;

        public ListarModel(IClaimsManager claimsManager,
                                    IApontamentoRepository apontamentoRepository,
                                    IRecursoTarefaRepository recursoTarefaRepository)
        {
            _claimsManager = claimsManager;

            _apontamentoRepository = apontamentoRepository;
            _recursoTarefaRepository = recursoTarefaRepository;
        }

        [BindProperty]
        public ApontamentoModel Apontamento { get; set; }

        public IEnumerable<ApontamentoModel> Lista { get; set; }

        public IEnumerable<RecursoTarefaModel> ListaRecursoTarefas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string retorno = _claimsManager.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
            int idRecurso = int.Parse(retorno);

            Lista = await _apontamentoRepository.ListarPoridRecursoAsync(idRecurso);
            ListaRecursoTarefas = await _recursoTarefaRepository.ListarPoridRecursoAsync(idRecurso);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _apontamentoRepository.ApontarHorasAsync(Apontamento);

            return RedirectToPage("Listar");
        }
    }
}