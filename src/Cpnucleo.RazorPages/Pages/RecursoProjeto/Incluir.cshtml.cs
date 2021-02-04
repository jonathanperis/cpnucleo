using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
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
        private readonly IRecursoProjetoService _recursoProjetoService;
        private readonly IRecursoService _recursoService;
        private readonly ICrudService<ProjetoViewModel> _projetoService;

        public IncluirModel(IRecursoProjetoService recursoProjetoService,
                            IRecursoService recursoService,
                            ICrudService<ProjetoViewModel> projetoService)
        {
            _recursoProjetoService = recursoProjetoService;
            _recursoService = recursoService;
            _projetoService = projetoService;
        }

        [BindProperty]
        public RecursoProjetoViewModel RecursoProjeto { get; set; }

        public ProjetoViewModel Projeto { get; set; }

        public SelectList SelectRecursos { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid idProjeto)
        {
            try
            {
                var result = await _projetoService.ConsultarAsync(Token, idProjeto);

                if (!result.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                    return Page();
                }

                Projeto = result.response;

                var result2 = await _recursoService.ListarAsync(Token);

                if (!result2.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result2.code} - {result2.message}");
                    return Page();
                }

                SelectRecursos = new SelectList(result2.response, "Id", "Nome");

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
                    var result = await _projetoService.ConsultarAsync(Token, idProjeto);

                    if (!result.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                        return Page();
                    }

                    Projeto = result.response;

                    var result2 = await _recursoService.ListarAsync(Token);

                    if (!result2.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result2.code} - {result2.message}");
                        return Page();
                    }

                    SelectRecursos = new SelectList(result2.response, "Id", "Nome");

                    return Page();
                }

                var result3 = await _recursoProjetoService.IncluirAsync(Token, RecursoProjeto);

                if (!result3.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result3.code} - {result3.message}");
                    return Page();
                }

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