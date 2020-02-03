using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cpnucleo.RazorPages.Pages.Projeto
{
    [Authorize]
    public class RemoverModel : PageBase
    {
        private readonly IProjetoApiService _projetoApiService;

        public RemoverModel(IClaimsManager claimsManager,
                                    IProjetoApiService projetoApiService)
            : base(claimsManager)
        {
            _projetoApiService = projetoApiService;
        }

        [BindProperty]
        public ProjetoViewModel Projeto { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Projeto = _projetoApiService.Consultar(Token, id);

            return Page();
        }

        public IActionResult OnPost()
        {
            _projetoApiService.Remover(Token, Projeto.Id);

            return RedirectToPage("Listar");
        }
    }
}