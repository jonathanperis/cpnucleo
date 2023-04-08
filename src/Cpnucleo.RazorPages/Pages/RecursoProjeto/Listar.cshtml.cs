using Cpnucleo.Shared.Queries.ListRecursoProjetoByProjeto;

namespace Cpnucleo.RazorPages.Pages.RecursoProjeto;

[Authorize]
public class ListarModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public ListarModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public RecursoProjetoDTO RecursoProjeto { get; set; }

    public IEnumerable<RecursoProjetoDTO> Lista { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid idProjeto)
    {
        try
        {
            ListRecursoProjetoByProjetoViewModel result = await _cpnucleoApiClient.ExecuteQueryAsync<ListRecursoProjetoByProjetoViewModel>("RecursoProjeto", "GetRecursoProjetoByProjeto", new ListRecursoProjetoByProjetoQuery(idProjeto));

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return Page();
            }

            Lista = result.RecursoProjetos;

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
