using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Cpnucleo.RazorPages.GRPC.Pages.Apontamento
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly IClaimsManager _claimsManager;
        private readonly IApontamentoApiService _apontamentoApiService;
        private readonly IRecursoTarefaApiService _recursoTarefaApiService;

        public ListarModel(IClaimsManager claimsManager,
                                    IApontamentoApiService apontamentoApiService,
                                    IRecursoTarefaApiService recursoTarefaApiService)
            : base(claimsManager)
        {
            _claimsManager = claimsManager;
            _apontamentoApiService = apontamentoApiService;
            _recursoTarefaApiService = recursoTarefaApiService;
        }

        [BindProperty]
        public ApontamentoViewModel Apontamento { get; set; }

        public IEnumerable<ApontamentoViewModel> Lista { get; set; }

        public IEnumerable<RecursoTarefaViewModel> ListaRecursoTarefas { get; set; }

        public IActionResult OnGet()
        {
            string retorno = _claimsManager.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
            Guid idRecurso = new Guid(retorno);

            Lista = _apontamentoApiService.ListarPorRecurso(Token, idRecurso);
            ListaRecursoTarefas = _recursoTarefaApiService.ListarPorRecurso(Token, idRecurso);

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                string retorno = _claimsManager.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
                Guid idRecurso = new Guid(retorno);

                Lista = _apontamentoApiService.ListarPorRecurso(Token, idRecurso);
                ListaRecursoTarefas = _recursoTarefaApiService.ListarPorRecurso(Token, idRecurso);

                return Page();
            }

            _apontamentoApiService.Incluir(Token, Apontamento);

            return RedirectToPage("Listar");
        }
    }
}