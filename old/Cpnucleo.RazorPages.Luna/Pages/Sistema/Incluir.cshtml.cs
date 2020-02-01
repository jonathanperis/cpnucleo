using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cpnucleo.RazorPages.Luna.Pages.Sistema
{
    [Authorize]
    public class IncluirModel : PageBase
    {
        private readonly ISistemaApiService _sistemaApiService;

        public IncluirModel(IClaimsManager claimsManager,
                                    ISistemaApiService sistemaApiService)
            : base(claimsManager)
        {
            _sistemaApiService = sistemaApiService;
        }

        [BindProperty]
        public SistemaViewModel Sistema { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _sistemaApiService.Incluir(Token, Sistema);

            return RedirectToPage("Listar");
        }
    }
}