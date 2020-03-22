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
    public class RemoverModel : PageModel
    {
        private readonly ICrudGrpcService<SistemaViewModel> _sistemaGrpcService;

        public RemoverModel(ICrudGrpcService<SistemaViewModel> sistemaGrpcService)
        {
            _sistemaGrpcService = sistemaGrpcService;
        }

        [BindProperty]
        public SistemaViewModel Sistema { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Sistema = await _sistemaGrpcService.ConsultarAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _sistemaGrpcService.RemoverAsync(Sistema.Id);

            return RedirectToPage("Listar");
        }
    }
}