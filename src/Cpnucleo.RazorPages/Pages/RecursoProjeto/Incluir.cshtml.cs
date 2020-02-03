using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Cpnucleo.RazorPages.Pages.RecursoProjeto
{
    [Authorize]
    public class IncluirModel : PageBase
    {
        private readonly IRecursoProjetoApiService _recursoProjetoApiService;
        private readonly IRecursoApiService _recursoApiService;
        private readonly IProjetoApiService _projetoApiService;

        public IncluirModel(IClaimsManager claimsManager,
                                    IRecursoProjetoApiService recursoProjetoApiService,
                                    IRecursoApiService recursoApiService,
                                    IProjetoApiService projetoApiService)
            : base(claimsManager)
        {
            _recursoProjetoApiService = recursoProjetoApiService;
            _recursoApiService = recursoApiService;
            _projetoApiService = projetoApiService;
        }

        [BindProperty]
        public RecursoProjetoViewModel RecursoProjeto { get; set; }

        public ProjetoViewModel Projeto { get; set; }

        public SelectList SelectRecursos { get; set; }

        public IActionResult OnGet(Guid idProjeto)
        {
            Projeto = _projetoApiService.Consultar(Token, idProjeto);
            SelectRecursos = new SelectList(_recursoApiService.Listar(Token), "Id", "Nome");

            return Page();
        }

        public IActionResult OnPost(Guid idProjeto)
        {
            if (!ModelState.IsValid)
            {
                Projeto = _projetoApiService.Consultar(Token, idProjeto);
                SelectRecursos = new SelectList(_recursoApiService.Listar(Token), "Id", "Nome");

                return Page();
            }

            _recursoProjetoApiService.Incluir(Token, RecursoProjeto);

            return RedirectToPage("Listar", new { idProjeto });
        }
    }
}