using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Impedimento
{
    [Authorize]
    public class IncluirModel : PageBase
    {
        private readonly IImpedimentoApiService _impedimentoApiService;

        public IncluirModel(IClaimsManager claimsManager,
                                    IImpedimentoApiService impedimentoApiService)
            : base(claimsManager)
        {
            _impedimentoApiService = impedimentoApiService;
        }

        [BindProperty]
        public ImpedimentoViewModel Impedimento { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _impedimentoApiService.Incluir(Token, Impedimento);

            return RedirectToPage("Listar");
        }
    }
}