using Cpnucleo.Infra.CrossCutting.Communication.GRPC.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.GRPC.Pages.Workflow
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly ICrudGrpcService<WorkflowViewModel> _workflowGrpcService;

        public RemoverModel(ICrudGrpcService<WorkflowViewModel> workflowGrpcService)
        {
            _workflowGrpcService = workflowGrpcService;
        }

        [BindProperty]
        public WorkflowViewModel Workflow { get; set; }

        public async Task<IActionResult> OnGet(Guid id)
        {
            Workflow = await _workflowGrpcService.ConsultarAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            await _workflowGrpcService.RemoverAsync(Workflow.Id);

            return RedirectToPage("Listar");
        }
    }
}