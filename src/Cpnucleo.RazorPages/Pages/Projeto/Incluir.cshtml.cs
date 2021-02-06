using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Projeto
{
    [Authorize]
    public class IncluirModel : PageBase
    {
        private readonly ICrudService<ProjetoViewModel> _projetoService;
        private readonly ICrudService<SistemaViewModel> _sistemaService;

        public IncluirModel(ICrudService<ProjetoViewModel> projetoService,
                            ICrudService<SistemaViewModel> sistemaService)
        {
            _projetoService = projetoService;
            _sistemaService = sistemaService;
        }

        [BindProperty]
        public ProjetoViewModel Projeto { get; set; }

        public SelectList SelectSistemas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var result = await _sistemaService.ListarAsync(Token);

                if (!result.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                    return Page();
                }         

                SelectSistemas = new SelectList(result.response, "Id", "Nome");

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var result = await _sistemaService.ListarAsync(Token);

                    if (!result.sucess)
                    {
                        ModelState.AddModelError(string.Empty, $"{result.code} - {result.message}");
                        return Page();
                    }         

                    SelectSistemas = new SelectList(result.response, "Id", "Nome");

                    return Page();
                }

                var result2 = await _projetoService.IncluirAsync(Token, Projeto);

                if (!result2.sucess)
                {
                    ModelState.AddModelError(string.Empty, $"{result2.code} - {result2.message}");
                    return Page();
                }

                return RedirectToPage("Listar");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}