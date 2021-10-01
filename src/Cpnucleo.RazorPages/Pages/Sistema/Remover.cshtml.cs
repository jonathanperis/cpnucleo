namespace Cpnucleo.RazorPages.Pages.Sistema;

[Authorize]
public class RemoverModel : PageBase
{
    private readonly ICpnucleoApiService _cpnucleoApiService;

    public RemoverModel(ICpnucleoApiService cpnucleoApiService)
    {
        _cpnucleoApiService = cpnucleoApiService;
    }

    [BindProperty]
    public SistemaViewModel Sistema { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        try
        {
            Sistema = await _cpnucleoApiService.GetAsync<SistemaViewModel>("sistema", Token, id);

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
                Sistema = await _cpnucleoApiService.GetAsync<SistemaViewModel>("sistema", Token, Sistema.Id);

                return Page();
            }

            await _cpnucleoApiService.DeleteAsync("sistema", Token, Sistema.Id);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
