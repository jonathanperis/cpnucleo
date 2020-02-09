using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.RecursoProjeto
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IRecursoProjetoGrpcService _recursoProjetoGrpcService;
        private readonly IRecursoGrpcService _recursoGrpcService;
        private readonly ICrudGrpcService<ProjetoViewModel> _projetoGrpcService;

        public IncluirModel(IRecursoProjetoGrpcService recursoProjetoGrpcService,
                                    IRecursoGrpcService recursoGrpcService,
                                    ICrudGrpcService<ProjetoViewModel> projetoGrpcService)
        {
            _recursoProjetoGrpcService = recursoProjetoGrpcService;
            _recursoGrpcService = recursoGrpcService;
            _projetoGrpcService = projetoGrpcService;
        }

        [BindProperty]
        public RecursoProjetoViewModel RecursoProjeto { get; set; }

        public ProjetoViewModel Projeto { get; set; }

        public SelectList SelectRecursos { get; set; }

        public async Task<IActionResult> OnGet(Guid idProjeto)
        {
            Projeto = await _projetoGrpcService.ConsultarAsync(idProjeto);
            SelectRecursos = new SelectList(await _recursoGrpcService.ListarAsync(), "Id", "Nome");

            return Page();
        }

        public async Task<IActionResult> OnPost(Guid idProjeto)
        {
            if (!ModelState.IsValid)
            {
                Projeto = await _projetoGrpcService.ConsultarAsync(idProjeto);
                SelectRecursos = new SelectList(await _recursoGrpcService.ListarAsync(), "Id", "Nome");

                return Page();
            }

            await _recursoProjetoGrpcService.IncluirAsync(RecursoProjeto);

            return RedirectToPage("Listar", new { idProjeto });
        }
    }
}