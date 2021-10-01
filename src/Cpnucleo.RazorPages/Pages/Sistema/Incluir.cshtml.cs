namespace Cpnucleo.RazorPages.Pages.Sistema;

[Authorize]
public class IncluirModel : PageBase
{
    private readonly ICpnucleoApiService _cpnucleoApiService;

    public IncluirModel(ICpnucleoApiService cpnucleoApiService)
    {
        _cpnucleoApiService = cpnucleoApiService;
    }

    [BindProperty]
    public SistemaViewModel Sistema { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _cpnucleoApiService.PostAsync<SistemaViewModel>("sistema", Token, Sistema);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
