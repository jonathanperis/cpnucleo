using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Recurso
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IRecursoGrpcService _recursoGrpcService;

        public ListarModel(IRecursoGrpcService recursoGrpcService)
        {
            _recursoGrpcService = recursoGrpcService;
        }

        public RecursoViewModel Recurso { get; set; }

        public IEnumerable<RecursoViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Lista = await _recursoGrpcService.ListarAsync();

            return Page();
        }
    }
}