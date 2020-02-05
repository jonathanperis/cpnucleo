using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Impedimento
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IImpedimentoGrpcService _impedimentoGrpcService;

        public RemoverModel(IImpedimentoGrpcService impedimentoGrpcService)
        {
            _impedimentoGrpcService = impedimentoGrpcService;
        }

        [BindProperty]
        public ImpedimentoViewModel Impedimento { get; set; }

        public async Task<IActionResult> OnGet(Guid id)
        {
            Impedimento = await _impedimentoGrpcService.ConsultarAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            await _impedimentoGrpcService.RemoverAsync(Impedimento.Id);

            return RedirectToPage("Listar");
        }
    }
}