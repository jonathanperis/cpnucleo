namespace Cpnucleo.RazorPages.Pages.Recurso;

[Authorize]
public class ListarModel : PageBase
{
    private readonly ICpnucleoApiService _cpnucleoApiService;

    public ListarModel(ICpnucleoApiService cpnucleoApiService)
    {
        _cpnucleoApiService = cpnucleoApiService;
    }

    public RecursoViewModel Recurso { get; set; }

    public IEnumerable<RecursoViewModel> Lista { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Lista = await _cpnucleoApiService.GetAsync<IEnumerable<RecursoViewModel>>("recurso", Token);

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
