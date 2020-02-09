using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cpnucleo.RazorPages.Pages.Sistema
{
    [Authorize]
    public class AlterarModel : PageBase
    {
        private readonly ICrudApiService<SistemaViewModel> _sistemaApiService;

        public AlterarModel(IClaimsManager claimsManager,
                                    ICrudApiService<SistemaViewModel> sistemaApiService)
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _sistemaApiService.Alterar(Token, Sistema);

            return RedirectToPage("Listar");
        }
    }
}