using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cpnucleo.RazorPages.Pages.Workflow
{
    [Authorize]
    public class IncluirModel : PageBase
    {
        private readonly ICrudApiService<WorkflowViewModel> _workflowApiService;

        public IncluirModel(IClaimsManager claimsManager,
                                    ICrudApiService<WorkflowViewModel> workflowApiService)
            : base(claimsManager)
        {
            _workflowApiService = workflowApiService;
        }

        [BindProperty]
        public WorkflowViewModel Workflow { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _workflowApiService.Incluir(Token, Workflow);

            return RedirectToPage("Listar");
        }
    }
}