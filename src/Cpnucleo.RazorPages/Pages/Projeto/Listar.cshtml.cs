namespace Cpnucleo.RazorPages.Pages.Projeto;

[Authorize]
public class ListarModel : PageBase
{
    private readonly ICpnucleoApiService _cpnucleoApiService;

    public ListarModel(ICpnucleoApiService cpnucleoApiService)
    {
        _cpnucleoApiService = cpnucleoApiService;
    }

    public ProjetoViewModel Projeto { get; set; }

    public IEnumerable<ProjetoViewModel> Lista { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Lista = await _cpnucleoApiService.GetAsync<IEnumerable<ProjetoViewModel>>("projeto", Token, true);

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
