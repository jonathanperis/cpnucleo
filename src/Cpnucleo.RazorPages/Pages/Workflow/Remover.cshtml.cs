using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Workflow
{
    [Authorize]
    public class RemoverModel : PageBase
    {
        private readonly ICrudApiService<WorkflowViewModel> _workflowApiService;

        public RemoverModel(IClaimsManager claimsManager,
                                    ICrudApiService<WorkflowViewModel> workflowApiService)
            : base(claimsManager)
        {
            _workflowApiService = workflowApiService;
        }

        [BindProperty]
        public WorkflowViewModel Workflow { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            try
            {
                Workflow = await _workflowApiService.ConsultarAsync(Token, id);

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
                await _workflowApiService.RemoverAsync(Token, Workflow.Id);

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