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
    public class AlterarModel : PageModel
    {
        private readonly ICrudGrpcService<WorkflowViewModel> _workflowGrpcService;

        public AlterarModel(ICrudGrpcService<WorkflowViewModel> workflowGrpcService)
        {
            _workflowGrpcService = workflowGrpcService;
        }

        [BindProperty]
        public WorkflowViewModel Workflow { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Workflow = await _workflowGrpcService.ConsultarAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _workflowGrpcService.AlterarAsync(Workflow);

            return RedirectToPage("Listar");
        }
    }
}