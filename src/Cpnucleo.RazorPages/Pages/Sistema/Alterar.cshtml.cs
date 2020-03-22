using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Sistema = await _sistemaApiService.ConsultarAsync(Token, id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _sistemaApiService.AlterarAsync(Token, Sistema);

            return RedirectToPage("Listar");
        }
    }
}