using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Pages.Workflow
{
    [Authorize]
    public class IncluirModel : PageModel
    {
        private readonly IWorkflowRepository _workflowRepository;

        public IncluirModel(IWorkflowRepository workflowRepository) => _workflowRepository = workflowRepository;

        [BindProperty]
        public WorkflowModel Workflow { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            await _workflowRepository.IncluirAsync(Workflow);

            return RedirectToPage("Listar");
        }
    }
}