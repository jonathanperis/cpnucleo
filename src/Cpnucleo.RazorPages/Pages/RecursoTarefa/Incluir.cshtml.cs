using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.RecursoTarefa
{
    [Authorize]
    public class IncluirModel : PageBase
    {
        private readonly ICpnucleoApiService _cpnucleoApiService;

        public IncluirModel(ICpnucleoApiService cpnucleoApiService)
        {
            _cpnucleoApiService = cpnucleoApiService;
        }

        [BindProperty]
        public RecursoTarefaViewModel RecursoTarefa { get; set; }

        public TarefaViewModel Tarefa { get; set; }

        public SelectList SelectRecursos { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid idTarefa)
        {
            try
            {
                Tarefa = await _cpnucleoApiService.GetAsync<TarefaViewModel>("tarefa", Token, idTarefa);

                IEnumerable<RecursoProjetoViewModel> result = await _cpnucleoApiService.GetAsync<IEnumerable<RecursoProjetoViewModel>>("recursoProjeto/getByProjeto", Token, Tarefa.IdProjeto);
                SelectRecursos = new SelectList(result, "Recurso.Id", "Recurso.Nome");

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
                    Tarefa = await _cpnucleoApiService.GetAsync<TarefaViewModel>("tarefa", Token, RecursoTarefa.IdTarefa);

                    IEnumerable<RecursoProjetoViewModel> result = await _cpnucleoApiService.GetAsync<IEnumerable<RecursoProjetoViewModel>>("recursoProjeto/getByProjeto", Token, Tarefa.IdProjeto);
                    SelectRecursos = new SelectList(result, "Recurso.Id", "Recurso.Nome");

                    return Page();
                }

                await _cpnucleoApiService.PostAsync<RecursoTarefaViewModel>("recursoTarefa", Token, RecursoTarefa);

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