using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cpnucleo.RazorPages.Pages.Apontamento
{
    [Authorize]
    public class RemoverModel : PageBase
    {
        private readonly IApontamentoApiService _apontamentoApiService;

        public RemoverModel(IClaimsManager claimsManager,
                                    IApontamentoApiService apontamentoApiService)
            : base(claimsManager)
        {
            _apontamentoApiService = apontamentoApiService;
        }

        [BindProperty]
        public ApontamentoViewModel Apontamento { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Apontamento = _apontamentoApiService.Consultar(Token, id);

            return Page();
        }

        public IActionResult OnPost()
        {
            _apontamentoApiService.Remover(Token, Apontamento.Id);

            return RedirectToPage("Listar");
        }
    }
}