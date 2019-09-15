using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace Cpnucleo.RazorPages.Pages.Workflow
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly IWorkflowAppService _workflowAppService;

        public AlterarModel(IWorkflowAppService workflowAppService)
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _workflowAppService.Alterar(Workflow);

            return RedirectToPage("Listar");
        }
    }
}