using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Projeto
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly ICrudService<ProjetoViewModel> _projetoService;

        public ListarModel(ICrudService<ProjetoViewModel> projetoService)
        {
            _projetoService = projetoService;
        }

        public ProjetoViewModel Projeto { get; set; }

        public IEnumerable<ProjetoViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Lista = await _projetoService.ListarAsync(Token, true);

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }
    }
}