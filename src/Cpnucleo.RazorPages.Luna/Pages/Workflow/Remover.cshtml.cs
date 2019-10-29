using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Luna.Pages.Workflow
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IWorkflowAppService _workflowAppService;

        public RemoverModel(IWorkflowAppService workflowAppService)
        {
            _workflowAppService = workflowAppService;
        }

        [BindProperty]
        public WorkflowViewModel Workflow { get; set; }

        public IActionResult OnGet(Guid id)
        {
            Workflow = _workflowAppService.Consultar(id);

            return Page();
        }

        public IActionResult OnPost()
        {
            _workflowAppService.Remover(Workflow.Id);

            return RedirectToPage("Listar");
        }
    }
}