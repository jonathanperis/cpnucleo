using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Sistema
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly ICrudGrpcService<SistemaViewModel> _sistemaGrpcService;

        public ListarModel(ICrudGrpcService<SistemaViewModel> sistemaGrpcService)
        {
            _sistemaGrpcService = sistemaGrpcService;
        }

        public SistemaViewModel Sistema { get; set; }

        public IEnumerable<SistemaViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Lista = await _sistemaGrpcService.ListarAsync();

            return Page();
        }
    }
}