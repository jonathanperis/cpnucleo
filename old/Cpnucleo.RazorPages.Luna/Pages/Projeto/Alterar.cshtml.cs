using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Cpnucleo.RazorPages.Luna.Pages.Projeto
{
    [Authorize]
    public class AlterarModel : PageBase
    {
        private readonly IProjetoApiService _projetoApiService;
        private readonly ISistemaApiService _sistemaApiService;

        public AlterarModel(IClaimsManager claimsManager,
                                    IProjetoApiService projetoApiService,
                                    ISistemaApiService sistemaApiService)
            : base(claimsManager)
        {
            _projetoApiService = projetoApiService;
            _sistemaApiService = sistemaApiService;
        }

        [BindProperty]
        public ProjetoViewModel Projeto { get; set; }

        public SelectList SelectSistemas { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Projeto = _projetoApiService.Consultar(Token, id);
            SelectSistemas = new SelectList(_sistemaApiService.Listar(Token), "Id", "Nome");

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                SelectSistemas = new SelectList(_sistemaApiService.Listar(Token), "Id", "Nome");

                return Page();
            }

            _projetoApiService.Alterar(Token, Projeto);

            return RedirectToPage("Listar");
        }
    }
}