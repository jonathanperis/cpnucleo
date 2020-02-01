using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cpnucleo.RazorPages.Luna.Pages.Sistema
{
    [Authorize]
    public class RemoverModel : PageBase
    {
        private readonly ISistemaApiService _sistemaApiService;

        public RemoverModel(IClaimsManager claimsManager,
                                    ISistemaApiService sistemaApiService)
            : base(claimsManager)
        {
            _sistemaApiService = sistemaApiService;
        }

        [BindProperty]
        public SistemaViewModel Sistema { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Sistema = _sistemaApiService.Consultar(Token, id);

            return Page();
        }

        public IActionResult OnPost()
        {
            _sistemaApiService.Remover(Token, Sistema.Id);

            return RedirectToPage("Listar");
        }
    }
}