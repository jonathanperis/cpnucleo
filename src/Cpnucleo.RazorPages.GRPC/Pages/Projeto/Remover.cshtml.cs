using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Projeto
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IProjetoGrpcService _projetoGrpcService;

        public RemoverModel(IProjetoGrpcService projetoGrpcService)
        {
            _projetoGrpcService = projetoGrpcService;
        }

        [BindProperty]
        public ProjetoViewModel Projeto { get; set; }

        public async Task<IActionResult> OnGet(Guid id)
        {
            Projeto = await _projetoGrpcService.ConsultarAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            await _projetoGrpcService.RemoverAsync(Projeto.Id);

            return RedirectToPage("Listar");
        }
    }
}