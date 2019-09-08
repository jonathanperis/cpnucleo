using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Cpnucleo.RazorPages.Pages.Apontamento
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IClaimsManager _claimsManager;

        private readonly IApontamentoAppService _apontamentoAppService;
        private readonly IRecursoTarefaAppService _recursoTarefaAppService;

        public ListarModel(IClaimsManager claimsManager,
                                    IApontamentoAppService apontamentoAppService,
                                    IRecursoTarefaAppService recursoTarefaAppService)
        {
            _claimsManager = claimsManager;

            _apontamentoAppService = apontamentoAppService;
            _recursoTarefaAppService = recursoTarefaAppService;
        }

        [BindProperty]
        public ApontamentoViewModel Apontamento { get; set; }

        public IEnumerable<ApontamentoViewModel> Lista { get; set; }

        public IEnumerable<RecursoTarefaViewModel> ListaRecursoTarefas { get; set; }

        public IActionResult OnGet()
        {
            string retorno = _claimsManager.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
            Guid idRecurso = new Guid(retorno);

            Lista = _apontamentoAppService.ListarPoridRecurso(idRecurso);
            ListaRecursoTarefas = _recursoTarefaAppService.ListarPoridRecurso(idRecurso);

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            _apontamentoAppService.ApontarHoras(Apontamento);

            return RedirectToPage("Listar");
        }
    }
}