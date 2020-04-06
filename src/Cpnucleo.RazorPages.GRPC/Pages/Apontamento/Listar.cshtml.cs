using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.GRPC.Services;
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
        private readonly IApontamentoGrpcService _apontamentoGrpcService;
        private readonly ITarefaGrpcService _tarefaGrpcService;

        public ListarModel(IApontamentoGrpcService apontamentoGrpcService,
                           ITarefaGrpcService tarefaGrpcService)
        {
            _apontamentoGrpcService = apontamentoGrpcService;
            _tarefaGrpcService = tarefaGrpcService;
        }

        [BindProperty]
        public ApontamentoViewModel Apontamento { get; set; }

        public IEnumerable<ApontamentoViewModel> Lista { get; set; }

        public IEnumerable<TarefaViewModel> ListaTarefas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string retorno = ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
            Guid idRecurso = new Guid(retorno);

            Lista = await _apontamentoGrpcService.ListarPorRecursoAsync(idRecurso);
            ListaTarefas = await _tarefaGrpcService.ListarPorRecursoAsync(idRecurso);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                string retorno = ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
                Guid idRecurso = new Guid(retorno);

                Lista = await _apontamentoGrpcService.ListarPorRecursoAsync(idRecurso);
                ListaTarefas = await _tarefaGrpcService.ListarPorRecursoAsync(idRecurso);

                return Page();
            }

            await _apontamentoGrpcService.IncluirAsync(Apontamento);

            return RedirectToPage("Listar");
        }
    }
}