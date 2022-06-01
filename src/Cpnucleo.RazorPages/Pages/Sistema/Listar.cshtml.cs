namespace Cpnucleo.RazorPages.Pages.Sistema;

//[Authorize]
public class ListarModel : PageBase
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
            var result = await _cpnucleoApiClient.ExecuteQueryAsync<ListSistemaViewModel>("Sistema", "ListSistema", Token, new ListSistemaQuery { GetDependencies = false });

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
