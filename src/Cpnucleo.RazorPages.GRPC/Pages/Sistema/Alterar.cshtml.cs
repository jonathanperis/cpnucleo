using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Sistema
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly ISistemaGrpcService _sistemaGrpcService;

        public AlterarModel(ISistemaGrpcService sistemaGrpcService)
        {
            _sistemaGrpcService = sistemaGrpcService;
        }

        [BindProperty]
        public SistemaViewModel Sistema { get; set; }

        public async Task<IActionResult> OnGet(Guid id)
        {
            Sistema = await _sistemaGrpcService.ConsultarAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _sistemaGrpcService.AlterarAsync(Sistema);

            return RedirectToPage("Listar");
        }
    }
}