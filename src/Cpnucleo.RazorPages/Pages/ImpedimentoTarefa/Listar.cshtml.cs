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
            var result = await _cpnucleoApiClient.ExecuteQueryAsync<GetImpedimentoTarefaByTarefaViewModel>("ImpedimentoTarefa", "GetImpedimentoTarefaByTarefa", Token, new GetImpedimentoTarefaByTarefaQuery { IdTarefa = idTarefa });

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return Page();
            }

            Lista = result.ImpedimentoTarefas;

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
