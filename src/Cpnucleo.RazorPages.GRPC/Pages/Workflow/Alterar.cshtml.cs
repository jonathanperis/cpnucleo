using Cpnucleo.Infra.CrossCutting.Communication.API.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Workflow
{
    [Authorize]
    public class AlterarModel : PageBase
    {
        private readonly IWorkflowApiService _workflowApiService;

        public AlterarModel(IClaimsManager claimsManager,
                                    IWorkflowApiService workflowApiService)
            : base(claimsManager)
        {
            _workflowApiService = workflowApiService;
        }

        [BindProperty]
        public WorkflowViewModel Workflow { get; set; }

        public async Task<IActionResult> OnGet(Guid id)
        {
            Workflow = _workflowApiService.Consultar(Token, id);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _workflowApiService.Alterar(Token, Workflow);

            return RedirectToPage("Listar");
        }
    }
}