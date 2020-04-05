using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.RecursoProjeto
{
    [Authorize]
    public class IncluirModel : PageBase
    {
        private readonly IRecursoProjetoApiService _recursoProjetoApiService;
        private readonly IRecursoApiService _recursoApiService;
        private readonly ICrudApiService<ProjetoViewModel> _projetoApiService;

        public IncluirModel(IClaimsManager claimsManager,
                                    IRecursoProjetoApiService recursoProjetoApiService,
                                    IRecursoApiService recursoApiService,
                                    ICrudApiService<ProjetoViewModel> projetoApiService)
            : base(claimsManager)
        {
            _recursoProjetoApiService = recursoProjetoApiService;
            _recursoApiService = recursoApiService;
            _projetoApiService = projetoApiService;
        }

        [BindProperty]
        public RecursoProjetoViewModel RecursoProjeto { get; set; }

        public ProjetoViewModel Projeto { get; set; }

        public SelectList SelectRecursos { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid idProjeto)
        {
            try
            {
                Projeto = await _projetoApiService.ConsultarAsync(Token, idProjeto);
                SelectRecursos = new SelectList(await _recursoApiService.ListarAsync(Token), "Id", "Nome");

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid idProjeto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Projeto = await _projetoApiService.ConsultarAsync(Token, idProjeto);
                    SelectRecursos = new SelectList(await _recursoApiService.ListarAsync(Token), "Id", "Nome");

                    return Page();
                }

                await _recursoProjetoApiService.IncluirAsync(Token, RecursoProjeto);

                return RedirectToPage("Listar", new { idProjeto });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}