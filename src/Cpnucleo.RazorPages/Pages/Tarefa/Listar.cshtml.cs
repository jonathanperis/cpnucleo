using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cpnucleo.RazorPages.Pages.Tarefa
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly ICpnucleoApiService _cpnucleoApiService;

        public ListarModel(ICpnucleoApiService cpnucleoApiService)
        {
            _cpnucleoApiService = cpnucleoApiService;
        }

        public TarefaViewModel Tarefa { get; set; }

        public IEnumerable<TarefaViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Lista = await _cpnucleoApiService.GetAsync<IEnumerable<TarefaViewModel>>("tarefa", Token, true);

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