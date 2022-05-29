namespace Cpnucleo.RazorPages.Pages.Sistema;

//[Authorize]
public class AlterarModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public AlterarModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public SistemaDTO Sistema { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        try
        {
            Sistema = await _cpnucleoApiClient.GetAsync<SistemaDTO>("sistema", Token, id);

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
                Sistema = await _cpnucleoApiClient.GetAsync<SistemaDTO>("sistema", Token, Sistema.Id);

                return Page();
            }

            await _cpnucleoApiClient.PutAsync("sistema", Token, Sistema.Id, Sistema);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
