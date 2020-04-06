using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
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
        private readonly ICrudApiService<ProjetoViewModel> _projetoApiService;
        private readonly ICrudApiService<SistemaViewModel> _sistemaApiService;

        public IncluirModel(ICrudApiService<ProjetoViewModel> projetoApiService,
                            ICrudApiService<SistemaViewModel> sistemaApiService)
        {
            _projetoApiService = projetoApiService;
            _sistemaApiService = sistemaApiService;
        }

        [BindProperty]
        public ProjetoViewModel Projeto { get; set; }

        public SelectList SelectSistemas { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                SelectSistemas = new SelectList(await _sistemaApiService.ListarAsync(Token), "Id", "Nome");

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
                    SelectSistemas = new SelectList(await _sistemaApiService.ListarAsync(Token), "Id", "Nome");

                    return Page();
                }

                await _projetoApiService.IncluirAsync(Token, Projeto);

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