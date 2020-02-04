using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Apontamento
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IApontamentoGrpcService _apontamentoGrpcService;

        public RemoverModel(IApontamentoGrpcService apontamentoGrpcService)
        {
            _apontamentoGrpcService = apontamentoGrpcService;
        }

        [BindProperty]
        public ApontamentoViewModel Apontamento { get; set; }

        public async Task<IActionResult> OnGet(Guid id)
        {
            Apontamento = await _apontamentoGrpcService.ConsultarAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            await _apontamentoGrpcService.RemoverAsync(Apontamento.Id);

            return RedirectToPage("Listar");
        }
    }
}