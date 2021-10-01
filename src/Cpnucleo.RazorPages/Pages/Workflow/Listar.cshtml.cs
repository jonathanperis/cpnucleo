using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cpnucleo.RazorPages.Pages.Workflow
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly ICpnucleoApiService _cpnucleoApiService;

        public ListarModel(ICpnucleoApiService cpnucleoApiService)
        {
            _cpnucleoApiService = cpnucleoApiService;
        }

        public WorkflowViewModel Workflow { get; set; }

        public IEnumerable<WorkflowViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Lista = await _cpnucleoApiService.GetAsync<IEnumerable<WorkflowViewModel>>("workflow", Token);

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