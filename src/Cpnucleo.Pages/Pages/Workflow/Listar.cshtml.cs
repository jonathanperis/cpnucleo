using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.Workflow
{
    [Authorize]
    public class ListarModel : PageModel
    {
        private readonly IWorkflowRepository _workflowRepository;

        public ListarModel(IWorkflowRepository workflowRepository) => _workflowRepository = workflowRepository;

        [BindProperty]
        public WorkflowItem Workflow { get; set; }

        [BindProperty]
        public IEnumerable<WorkflowItem> Lista { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Lista = await _workflowRepository.Listar();

            return Page();
        }
    }
}