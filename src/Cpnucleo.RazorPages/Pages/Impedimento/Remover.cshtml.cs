namespace Cpnucleo.RazorPages.Pages.Impedimento;

[Authorize]
public class RemoverModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public RemoverModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public ImpedimentoDTO Impedimento { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        try
        {
            Impedimento = await _cpnucleoApiClient.GetAsync<ImpedimentoDTO>("impedimento", Token, id);

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
                Impedimento = await _cpnucleoApiClient.GetAsync<ImpedimentoDTO>("impedimento", Token, Impedimento.Id);

                return Page();
            }

            await _cpnucleoApiClient.DeleteAsync("impedimento", Token, Impedimento.Id);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
