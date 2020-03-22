using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Projeto
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly ICrudGrpcService<ProjetoViewModel> _projetoGrpcService;

        public ListarModel(ICrudGrpcService<ProjetoViewModel> projetoGrpcService)
        {
            _projetoGrpcService = projetoGrpcService;
        }

        public ProjetoViewModel Projeto { get; set; }

        public IEnumerable<ProjetoViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _projetoGrpcService.ListarAsync();

            return Page();
        }
    }
}