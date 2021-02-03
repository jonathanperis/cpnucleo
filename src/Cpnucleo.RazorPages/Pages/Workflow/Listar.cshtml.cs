using Cpnucleo.RazorPages.Services.Interfaces;
using Cpnucleo.RazorPages.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.RazorPages.Pages.Workflow
{
    [Authorize]
    public class ListarModel : PageBase
    {
        private readonly ICrudService<WorkflowViewModel> _workflowService;

        public ListarModel(ICrudService<WorkflowViewModel> workflowService)
        {
            _workflowService = workflowService;
        }

        public WorkflowViewModel Workflow { get; set; }

        public IEnumerable<WorkflowViewModel> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Lista = await _workflowService.ListarAsync(Token);

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