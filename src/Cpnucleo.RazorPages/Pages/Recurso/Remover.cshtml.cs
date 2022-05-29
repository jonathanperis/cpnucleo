namespace Cpnucleo.RazorPages.Pages.Recurso;

[Authorize]
public class RemoverModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public RemoverModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public RecursoDTO Recurso { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        try
        {
            Recurso = await _cpnucleoApiClient.GetAsync<RecursoDTO>("recurso", Token, id);

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
                Recurso = await _cpnucleoApiClient.GetAsync<RecursoDTO>("recurso", Token, Recurso.Id);

                return Page();
            }

            await _cpnucleoApiClient.DeleteAsync("recurso", Token, Recurso.Id);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
