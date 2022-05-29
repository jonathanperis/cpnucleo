namespace Cpnucleo.RazorPages.Pages.Workflow;

[Authorize]
public class RemoverModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public RemoverModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public WorkflowDTO Workflow { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        try
        {
            Workflow = await _cpnucleoApiClient.GetAsync<WorkflowDTO>("workflow", Token, id);

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                Workflow = await _cpnucleoApiClient.GetAsync<WorkflowDTO>("workflow", Token, Workflow.Id);

                return Page();
            }

            await _cpnucleoApiClient.DeleteAsync("workflow", Token, Workflow.Id);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
