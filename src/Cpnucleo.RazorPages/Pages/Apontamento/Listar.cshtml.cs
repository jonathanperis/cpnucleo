using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Cpnucleo.RazorPages.Pages.Apontamento
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly IClaimsManager _claimsManager;
        private readonly IApontamentoApiService _apontamentoApiService;
        private readonly ITarefaApiService _tarefaApiService;

        public ListarModel(IClaimsManager claimsManager,
                                    IApontamentoApiService apontamentoApiService,
                                    ITarefaApiService tarefaApiService)
            : base(claimsManager)
        {
            _claimsManager = claimsManager;
            _apontamentoApiService = apontamentoApiService;
            _tarefaApiService = tarefaApiService;
        }

        [BindProperty]
        public ApontamentoViewModel Apontamento { get; set; }

        public IEnumerable<ApontamentoViewModel> Lista { get; set; }

        public IEnumerable<TarefaViewModel> ListaTarefas { get; set; }

        public IActionResult OnGet()
        {
            string retorno = _claimsManager.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
            Guid idRecurso = new Guid(retorno);

            Lista = _apontamentoApiService.ListarPorRecurso(Token, idRecurso);
            ListaTarefas = _tarefaApiService.ListarPorRecurso(Token, idRecurso);

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                string retorno = _claimsManager.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
                Guid idRecurso = new Guid(retorno);

                Lista = _apontamentoApiService.ListarPorRecurso(Token, idRecurso);
                ListaTarefas = _tarefaApiService.ListarPorRecurso(Token, idRecurso);

                return Page();
            }

            _apontamentoApiService.Incluir(Token, Apontamento);

            return RedirectToPage("Listar");
        }
    }
}