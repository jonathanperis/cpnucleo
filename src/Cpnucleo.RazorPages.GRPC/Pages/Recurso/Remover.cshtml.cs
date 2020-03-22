using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Recurso
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IRecursoGrpcService _recursoGrpcService;

        public RemoverModel(IRecursoGrpcService recursoGrpcService)
        {
            _recursoGrpcService = recursoGrpcService;
        }

        [BindProperty]
        public RecursoViewModel Recurso { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Recurso = await _recursoGrpcService.ConsultarAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _recursoGrpcService.RemoverAsync(Recurso.Id);

            return RedirectToPage("Listar");
        }
    }
}