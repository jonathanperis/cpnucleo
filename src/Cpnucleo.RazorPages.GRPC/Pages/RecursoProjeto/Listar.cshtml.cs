using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.RecursoProjeto
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IRecursoProjetoGrpcService _recursoProjetoGrpcService;

        public ListarModel(IRecursoProjetoGrpcService recursoProjetoGrpcService)
        {
            _recursoProjetoGrpcService = recursoProjetoGrpcService;
        }

        [BindProperty]
        public RecursoProjetoViewModel RecursoProjeto { get; set; }

        public IEnumerable<RecursoProjetoViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid idProjeto)
        {
            Lista = await _recursoProjetoGrpcService.ListarPorProjetoAsync(idProjeto);

            ViewData["idProjeto"] = idProjeto;

            return Page();
        }
    }
}