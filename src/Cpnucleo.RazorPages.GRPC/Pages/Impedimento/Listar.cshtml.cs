using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Impedimento
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IImpedimentoGrpcService _impedimentoGrpcService;

        public ListarModel(IImpedimentoGrpcService impedimentoGrpcService)
        {
            _impedimentoGrpcService = impedimentoGrpcService;
        }

        public ImpedimentoViewModel Impedimento { get; set; }

        public IEnumerable<ImpedimentoViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Lista = await _impedimentoGrpcService.ListarAsync();

            return Page();
        }
    }
}