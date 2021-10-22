namespace Cpnucleo.RazorPages.Pages.RecursoTarefa;

[Authorize]
public class ListarModel : PageBase
{
    private readonly ICpnucleoApiService _cpnucleoApiService;

    public ListarModel(ICpnucleoApiService cpnucleoApiService)
    {
        _cpnucleoApiService = cpnucleoApiService;
    }

    [BindProperty]
    public RecursoTarefaViewModel RecursoTarefa { get; set; }

    public IEnumerable<RecursoTarefaViewModel> Lista { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid idTarefa)
    {
        try
        {
            Lista = await _cpnucleoApiService.GetAsync<IEnumerable<RecursoTarefaViewModel>>("recursoTarefa", "getByTarefa", Token, idTarefa);

            ViewData["idTarefa"] = idTarefa;

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
