using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.RecursoTarefa
{
    [Authorize]
    public class IncluirModel : PageBase
    {
        private readonly IRecursoTarefaApiService _recursoTarefaApiService;
        private readonly IRecursoProjetoApiService _recursoProjetoApiService;
        private readonly ITarefaApiService _tarefaApiService;

        public IncluirModel(IClaimsManager claimsManager,
                                    IRecursoTarefaApiService recursoTarefaApiService,
                                    IRecursoProjetoApiService recursoProjetoApiService,
                                    ITarefaApiService tarefaApiService)
            : base(claimsManager)
        {
            _recursoTarefaApiService = recursoTarefaApiService;
            _recursoProjetoApiService = recursoProjetoApiService;
            _tarefaApiService = tarefaApiService;
        }

        [BindProperty]
        public RecursoTarefaViewModel RecursoTarefa { get; set; }

        public TarefaViewModel Tarefa { get; set; }

        public SelectList SelectRecursos { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid idTarefa)
        {
            try
            {
                Tarefa = await _tarefaApiService.ConsultarAsync(Token, idTarefa);

                SelectRecursos = new SelectList(await _recursoProjetoApiService.ListarPorProjetoAsync(Token, Tarefa.IdProjeto), "Recurso.Id", "Recurso.Nome");

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid idTarefa)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Tarefa = await _tarefaApiService.ConsultarAsync(Token, idTarefa);
                    SelectRecursos = new SelectList(await _recursoProjetoApiService.ListarPorProjetoAsync(Token, Tarefa.IdProjeto), "Recurso.Id", "Recurso.Nome");

                    return Page();
                }

                await _recursoTarefaApiService.IncluirAsync(Token, RecursoTarefa);

                return RedirectToPage("Listar", new { idTarefa });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}