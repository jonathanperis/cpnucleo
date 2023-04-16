using Cpnucleo.Shared.Queries.ListRecursoTarefaByTarefa;

namespace Cpnucleo.RazorPages.Pages.RecursoTarefa;

[Authorize]
public class ListarModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public ListarModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public RecursoTarefaDto RecursoTarefa { get; set; }

    public IEnumerable<RecursoTarefaDto> Lista { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid idTarefa)
    {
        try
        {
            var result = await _cpnucleoApiClient.ExecuteAsync<ListRecursoTarefaByTarefaViewModel>("RecursoTarefa", "GetRecursoTarefaByTarefa", new ListRecursoTarefaByTarefaQuery(idTarefa));

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return Page();
            }

            Lista = result.RecursoTarefas;

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
