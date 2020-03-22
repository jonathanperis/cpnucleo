using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Projeto
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly ICrudGrpcService<ProjetoViewModel> _projetoGrpcService;
        private readonly ICrudGrpcService<SistemaViewModel> _sistemaGrpcService;

        public IncluirModel(ICrudGrpcService<ProjetoViewModel> projetoGrpcService,
                            ICrudGrpcService<SistemaViewModel> sistemaGrpcService)
        {
            _projetoGrpcService = projetoGrpcService;
            _sistemaGrpcService = sistemaGrpcService;
        }

        [BindProperty]
        public ProjetoViewModel Projeto { get; set; }

        public SelectList SelectSistemas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            SelectSistemas = new SelectList(await _sistemaGrpcService.ListarAsync(), "Id", "Nome");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                SelectSistemas = new SelectList(await _sistemaGrpcService.ListarAsync(), "Id", "Nome");

                return Page();
            }

            await _projetoGrpcService.IncluirAsync(Projeto);

            return RedirectToPage("Listar");
        }
    }
}