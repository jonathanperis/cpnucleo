using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Sistema
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly ISistemaGrpcService _sistemaGrpcService;

        public IncluirModel(ISistemaGrpcService sistemaGrpcService)
        {
            _sistemaGrpcService = sistemaGrpcService;
        }

        [BindProperty]
        public SistemaViewModel Sistema { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _sistemaGrpcService.IncluirAsync(Sistema);

            return RedirectToPage("Listar");
        }
    }
}