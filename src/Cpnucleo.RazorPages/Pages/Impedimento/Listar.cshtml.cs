namespace Cpnucleo.RazorPages.Pages.Impedimento;

[Authorize]
public class ListarModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public ListarModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    public ImpedimentoDTO Impedimento { get; set; }

    public IEnumerable<ImpedimentoDTO> Lista { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Lista = await _cpnucleoApiClient.GetAsync<IEnumerable<ImpedimentoDTO>>("impedimento", Token);

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
