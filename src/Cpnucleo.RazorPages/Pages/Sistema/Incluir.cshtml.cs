using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Sistema
{
    [Authorize]
    public class IncluirModel : PageBase
    {
        private readonly ICrudApiService<SistemaViewModel> _sistemaApiService;

        public IncluirModel(IClaimsManager claimsManager,
                                    ICrudApiService<SistemaViewModel> sistemaApiService)
            : base(claimsManager)
        {
            _sistemaApiService = sistemaApiService;
        }

        [BindProperty]
        public SistemaViewModel Sistema { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _sistemaApiService.IncluirAsync(Token, Sistema);

            return RedirectToPage("Listar");
        }
    }
}