namespace Cpnucleo.RazorPages.Pages.Workflow;

[Authorize]
public class IncluirModel : PageBase
{
    private readonly ICpnucleoApiService _cpnucleoApiService;

    public IncluirModel(ICpnucleoApiService cpnucleoApiService)
    {
        _cpnucleoApiService = cpnucleoApiService;
    }

    [BindProperty]
    public WorkflowViewModel Workflow { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _cpnucleoApiService.PostAsync<WorkflowViewModel>("workflow", Token, Workflow);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
