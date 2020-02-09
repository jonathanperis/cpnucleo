using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Impedimento
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly ICrudGrpcService<ImpedimentoViewModel> _impedimentoGrpcService;

        public IncluirModel(ICrudGrpcService<ImpedimentoViewModel> impedimentoGrpcService)
        {
            _impedimentoGrpcService = impedimentoGrpcService;
        }

        [BindProperty]
        public ImpedimentoViewModel Impedimento { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _impedimentoGrpcService.IncluirAsync(Impedimento);

            return RedirectToPage("Listar");
        }
    }
}