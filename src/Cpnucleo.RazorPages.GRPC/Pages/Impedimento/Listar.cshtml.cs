using Cpnucleo.Infra.CrossCutting.Communication.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.GRPC.Pages.Impedimento
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly IImpedimentoApiService _impedimentoApiService;

        public ListarModel(IClaimsManager claimsManager,
                                    IImpedimentoApiService impedimentoApiService)
            : base(claimsManager)
        {
            _impedimentoApiService = impedimentoApiService;
        }

        public ImpedimentoViewModel Impedimento { get; set; }

        public IEnumerable<ImpedimentoViewModel> Lista { get; set; }

        public IActionResult OnGet()
        {
            Lista = _impedimentoApiService.Listar(Token);

            return Page();
        }
    }
}