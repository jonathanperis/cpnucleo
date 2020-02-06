using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

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

        public IActionResult OnGet(Guid id)
        {
            Impedimento = _impedimentoApiService.Consultar(Token, id);

            return Page();
        }

        public IActionResult OnPost()
        {
            _impedimentoApiService.Remover(Token, Impedimento.Id);

            return RedirectToPage("Listar");
        }
    }
}