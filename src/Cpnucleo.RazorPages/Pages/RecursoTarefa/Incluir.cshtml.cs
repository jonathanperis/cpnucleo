using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
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
        private readonly IRecursoTarefaService _recursoTarefaService;
        private readonly IRecursoProjetoService _recursoProjetoService;
        private readonly ITarefaService _tarefaService;

        public IncluirModel(IRecursoTarefaService recursoTarefaService,
                            IRecursoProjetoService recursoProjetoService,
                            ITarefaService tarefaService)
        {
            _recursoTarefaService = recursoTarefaService;
            _recursoProjetoService = recursoProjetoService;
            _tarefaService = tarefaService;
        }

        [BindProperty]
        public RecursoTarefaViewModel RecursoTarefa { get; set; }

        public TarefaViewModel Tarefa { get; set; }

        public SelectList SelectRecursos { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid idTarefa)
        {
            try
            {
                Tarefa = await _tarefaService.ConsultarAsync(Token, idTarefa);

                SelectRecursos = new SelectList(await _recursoProjetoService.ListarPorProjetoAsync(Token, Tarefa.IdProjeto), "Recurso.Id", "Recurso.Nome");

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
                    Tarefa = await _tarefaService.ConsultarAsync(Token, idTarefa);
                    SelectRecursos = new SelectList(await _recursoProjetoService.ListarPorProjetoAsync(Token, Tarefa.IdProjeto), "Recurso.Id", "Recurso.Nome");

                    return Page();
                }

                await _recursoTarefaService.IncluirAsync(Token, RecursoTarefa);

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