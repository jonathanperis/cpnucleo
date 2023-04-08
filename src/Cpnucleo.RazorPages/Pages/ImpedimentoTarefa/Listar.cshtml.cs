using Cpnucleo.Shared.Queries.ListImpedimentoTarefaByTarefa;

namespace Cpnucleo.RazorPages.Pages.ImpedimentoTarefa;

[Authorize]
public class ListarModel : PageModel
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
            ListImpedimentoTarefaByTarefaViewModel result = await _cpnucleoApiClient.ExecuteAsync<ListImpedimentoTarefaByTarefaViewModel>("ImpedimentoTarefa", "GetImpedimentoTarefaByTarefa", new ListImpedimentoTarefaByTarefaQuery(idTarefa));

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
