using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Apontamento
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IClaimsManager _claimsManager;
        private readonly IApontamentoGrpcService _apontamentoGrpcService;
        private readonly ITarefaGrpcService _tarefaGrpcService;

        public ListarModel(IClaimsManager claimsManager,
                                IApontamentoGrpcService apontamentoGrpcService,
                                ITarefaGrpcService tarefaGrpcService)
        {
            _claimsManager = claimsManager;
            _apontamentoGrpcService = apontamentoGrpcService;
            _tarefaGrpcService = tarefaGrpcService;
        }

        [BindProperty]
        public ApontamentoViewModel Apontamento { get; set; }

        public IEnumerable<ApontamentoViewModel> Lista { get; set; }

        public IEnumerable<TarefaViewModel> ListaTarefas { get; set; }

        public async Task<IActionResult> OnGet()
        {
            string retorno = _claimsManager.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
            Guid idRecurso = new Guid(retorno);

            Lista = await _apontamentoGrpcService.ListarPorRecursoAsync(idRecurso);
            ListaTarefas = await _tarefaGrpcService.ListarAsync();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                string retorno = _claimsManager.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
                Guid idRecurso = new Guid(retorno);

                Lista = await _apontamentoGrpcService.ListarPorRecursoAsync(idRecurso);
                ListaTarefas = await _tarefaGrpcService.ListarAsync();

                return Page();
            }

            await _apontamentoGrpcService.IncluirAsync(Apontamento);

            return RedirectToPage("Listar");
        }
    }
}