using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Cpnucleo.RazorPages.Pages.Sistema
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly ICrudApiService<SistemaViewModel> _sistemaApiService;

        public ListarModel(IClaimsManager claimsManager,
                                    ICrudApiService<SistemaViewModel> sistemaApiService)
            : base(claimsManager)
        {
            _sistemaApiService = sistemaApiService;
        }

        public SistemaViewModel Sistema { get; set; }

        public IEnumerable<SistemaViewModel> Lista { get; set; }

        public IActionResult OnGet()
        {
            Lista = _sistemaApiService.Listar(Token);

            return Page();
        }
    }
}