using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Sistema
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly ICrudService<SistemaViewModel> _sistemaService;

        public ListarModel(ICrudService<SistemaViewModel> sistemaService)
        {
            _sistemaService = sistemaService;
        }

        public SistemaViewModel Sistema { get; set; }

        public IEnumerable<SistemaViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Lista = await _sistemaService.ListarAsync(Token);

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