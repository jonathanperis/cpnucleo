using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
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
                var result = await _tarefaService.ConsultarAsync(Token, idTarefa);

                if (!result.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                    return Page();
                }

                Tarefa = result.response;

                var result2 = await _recursoProjetoService.ListarPorProjetoAsync(Token, Tarefa.IdProjeto);

                if (!result2.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result2.code} - {result2.message}");
                    return Page();
                }

                SelectRecursos = new SelectList(result2.response, "Recurso.Id", "Recurso.Nome");

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
                    var result = await _tarefaService.ConsultarAsync(Token, RecursoTarefa.IdTarefa);

                    if (!result.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                        return Page();
                    }

                    Tarefa = result.response;

                    var result2 = await _recursoProjetoService.ListarPorProjetoAsync(Token, Tarefa.IdProjeto);

                    if (!result2.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result2.code} - {result2.message}");
                        return Page();
                    }

                    SelectRecursos = new SelectList(result2.response, "Recurso.Id", "Recurso.Nome");
                    
                    return Page();
                }

                var result3 = await _recursoTarefaService.IncluirAsync(Token, RecursoTarefa);

                if (!result3.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result3.code} - {result3.message}");
                    return Page();
                }

                return RedirectToPage("Listar", new { idTarefa = RecursoTarefa.IdTarefa });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}