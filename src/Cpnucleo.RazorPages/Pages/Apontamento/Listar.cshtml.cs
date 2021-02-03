using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using Cpnucleo.RazorPages.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Apontamento
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly IApontamentoService _apontamentoService;
        private readonly ITarefaService _tarefaService;

        public ListarModel(IApontamentoService apontamentoService,
                           ITarefaService tarefaService)
        {
            _apontamentoService = apontamentoService;
            _tarefaService = tarefaService;
        }

        [BindProperty]
        public ApontamentoViewModel Apontamento { get; set; }

        public IEnumerable<ApontamentoViewModel> Lista { get; set; }

        public IEnumerable<TarefaViewModel> ListaTarefas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                string retorno = ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
                Guid idRecurso = new Guid(retorno);

                Lista = await _apontamentoService.ListarPorRecursoAsync(Token, idRecurso);
                ListaTarefas = await _tarefaService.ListarPorRecursoAsync(Token, idRecurso);

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    string retorno = ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
                    Guid idRecurso = new Guid(retorno);

                    Lista = await _apontamentoService.ListarPorRecursoAsync(Token, idRecurso);
                    ListaTarefas = await _tarefaService.ListarPorRecursoAsync(Token, idRecurso);

                    return Page();
                }

                await _apontamentoService.IncluirAsync(Token, Apontamento);

                return RedirectToPage("Listar");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}