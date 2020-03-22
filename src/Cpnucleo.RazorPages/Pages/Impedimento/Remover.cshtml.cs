using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Impedimento
{
    [Authorize]
    public class RemoverModel : PageBase
    {
        private readonly ICrudApiService<ImpedimentoViewModel> _impedimentoApiService;

        public RemoverModel(IClaimsManager claimsManager,
                                    ICrudApiService<ImpedimentoViewModel> impedimentoApiService)
            : base(claimsManager)
        {
            _impedimentoApiService = impedimentoApiService;
        }

        [BindProperty]
        public ImpedimentoViewModel Impedimento { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Impedimento = await _impedimentoApiService.ConsultarAsync(Token, id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _impedimentoApiService.RemoverAsync(Token, Impedimento.Id);

            return RedirectToPage("Listar");
        }
    }
}