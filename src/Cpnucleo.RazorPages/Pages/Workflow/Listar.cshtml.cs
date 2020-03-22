using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Workflow
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly ICrudApiService<WorkflowViewModel> _workflowApiService;

        public ListarModel(IClaimsManager claimsManager,
                                    ICrudApiService<WorkflowViewModel> workflowApiService)
            : base(claimsManager)
        {
            _workflowApiService = workflowApiService;
        }

        public WorkflowViewModel Workflow { get; set; }

        public IEnumerable<WorkflowViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Lista = await _workflowApiService.ListarAsync(Token);

            return Page();
        }
    }
}