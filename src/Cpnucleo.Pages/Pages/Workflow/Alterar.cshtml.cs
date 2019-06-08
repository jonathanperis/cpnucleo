using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.Workflow
{
    [Authorize]
    public class AlterarModel : PageModel
    {
        private readonly IWorkflowRepository _workflowRepository;

        public AlterarModel(IWorkflowRepository workflowRepository) => _workflowRepository = workflowRepository;

        [BindProperty]
        public WorkflowItem Workflow { get; set; }

        public async Task<IActionResult> OnGetAsync(int idWorkflow)
        {
            Workflow = await _workflowRepository.Consultar(idWorkflow);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(WorkflowItem workflow)
        {
            if (!ModelState.IsValid) return Page();

            await _workflowRepository.Alterar(workflow);

            return RedirectToPage("Listar");
        }
    }
}