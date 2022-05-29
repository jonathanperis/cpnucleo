namespace Cpnucleo.RazorPages.Pages.ImpedimentoTarefa;

[Authorize]
public class ListarModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public ListarModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public ImpedimentoTarefaDTO ImpedimentoTarefa { get; set; }

    public IEnumerable<ImpedimentoTarefaDTO> Lista { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid idTarefa)
    {
        try
        {
            Lista = await _cpnucleoApiClient.GetAsync<IEnumerable<ImpedimentoTarefaDTO>>("impedimentoTarefa", "getByTarefa", Token, idTarefa);

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
