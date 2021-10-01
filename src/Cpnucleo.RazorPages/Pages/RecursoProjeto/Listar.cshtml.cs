namespace Cpnucleo.RazorPages.Pages.RecursoProjeto;

[Authorize]
public class ListarModel : PageBase
{
    private readonly ICpnucleoApiService _cpnucleoApiService;

    public ListarModel(ICpnucleoApiService cpnucleoApiService)
    {
        _cpnucleoApiService = cpnucleoApiService;
    }

    [BindProperty]
    public RecursoProjetoViewModel RecursoProjeto { get; set; }

    public IEnumerable<RecursoProjetoViewModel> Lista { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid idProjeto)
    {
        try
        {
            Lista = await _cpnucleoApiService.GetAsync<IEnumerable<RecursoProjetoViewModel>>("recursoProjeto/getByProjeto", Token, idProjeto);

            ViewData["idProjeto"] = idProjeto;

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
