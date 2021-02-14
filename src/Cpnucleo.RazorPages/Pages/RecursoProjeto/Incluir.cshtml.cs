﻿using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.Pages.RecursoProjeto
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
        public RecursoProjetoViewModel RecursoProjeto { get; set; }

        public ProjetoViewModel Projeto { get; set; }

        public SelectList SelectRecursos { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid idProjeto)
        {
            try
            {
                Projeto = await _cpnucleoApiService.GetAsync<ProjetoViewModel>("projeto", Token, idProjeto);

                var result = await _cpnucleoApiService.GetAsync<IEnumerable<RecursoViewModel>>("recurso", Token);
                SelectRecursos = new SelectList(result, "Id", "Nome");

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
                    Projeto = await _cpnucleoApiService.GetAsync<ProjetoViewModel>("projeto", Token, RecursoProjeto.IdProjeto);

                    var result = await _cpnucleoApiService.GetAsync<IEnumerable<RecursoViewModel>>("recurso", Token);
                    SelectRecursos = new SelectList(result, "Id", "Nome");

                    return Page();
                }

                await _cpnucleoApiService.PostAsync<RecursoProjetoViewModel>("recursoProjeto", Token, RecursoProjeto);

                return RedirectToPage("Listar", new { idProjeto = RecursoProjeto.IdProjeto });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}