using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Impedimento
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly ICrudService<ImpedimentoViewModel> _impedimentoService;

        public ListarModel(ICrudService<ImpedimentoViewModel> impedimentoService)
        {
            _impedimentoService = impedimentoService;
        }

        public ImpedimentoViewModel Impedimento { get; set; }

        public IEnumerable<ImpedimentoViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Lista = await _impedimentoService.ListarAsync(Token);

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