using Cpnucleo.Shared.Queries.ListSistema;

namespace Cpnucleo.RazorPages.Pages.Sistema;

[Authorize]
public class ListarModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public ListarModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    public SistemaDTO Sistema { get; set; }

    public IEnumerable<SistemaDTO> Lista { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            ListSistemaViewModel result = await _cpnucleoApiClient.ExecuteAsync<ListSistemaViewModel>("Sistema", "ListSistema", new ListSistemaQuery());

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return Page();
            }

            Lista = result.Sistemas;

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
