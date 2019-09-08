using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Pages.Workflow
{
    [Authorize]
    public class RemoverModel : PageModel
    {
        private readonly IWorkflowAppService _workflowAppService;

        public RemoverModel(IWorkflowAppService workflowAppService) => _workflowAppService = workflowAppService;

        [BindProperty]
        public WorkflowViewModel Workflow { get; set; }

        public IActionResult OnGet(Guid idWorkflow)
        {
            Workflow = _workflowAppService.Consultar(idWorkflow);

            return Page();
        }

        public IActionResult OnPost()
        {
            _workflowAppService.Remover(Workflow.Id);

            return RedirectToPage("Listar");
        }
    }
}