using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.Pages.RecursoTarefa
{
    [Authorize]
    public class IncluirModel : PageBase
    {
        private readonly IHttpService _httpService;

        public IncluirModel(IHttpService httpService)
        {
            _httpService = httpService;
        }

        [BindProperty]
        public RecursoTarefaViewModel RecursoTarefa { get; set; }

        public TarefaViewModel Tarefa { get; set; }

        public SelectList SelectRecursos { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid idTarefa)
        {
            try
            {
                var result = await _httpService.GetAsync<TarefaViewModel>("tarefa", Token, idTarefa);

                if (!result.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                    return Page();
                }

                Tarefa = result.response;

                var result2 = await _httpService.GetAsync<IEnumerable<RecursoProjetoViewModel>>("recursoProjeto/getByProjeto", Token, Tarefa.IdProjeto);

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
                    var result = await _httpService.GetAsync<TarefaViewModel>("tarefa", Token, RecursoTarefa.IdTarefa);

                    if (!result.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                        return Page();
                    }

                    Tarefa = result.response;

                    var result2 = await _httpService.GetAsync<IEnumerable<RecursoProjetoViewModel>>("recursoProjeto/getByProjeto", Token, Tarefa.IdProjeto);

                    if (!result2.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result2.code} - {result2.message}");
                        return Page();
                    }

                    SelectRecursos = new SelectList(result2.response, "Recurso.Id", "Recurso.Nome");
                    
                    return Page();
                }

                var result3 = await _httpService.PostAsync<RecursoTarefaViewModel>("recursoTarefa", Token, RecursoTarefa);

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