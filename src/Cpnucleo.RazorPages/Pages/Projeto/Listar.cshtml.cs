namespace Cpnucleo.RazorPages.Pages.Projeto;

[Authorize]
public class ListarModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public ListarModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    public ProjetoDTO Projeto { get; set; }

    public IEnumerable<ProjetoDTO> Lista { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            ListProjetoViewModel result = await _cpnucleoApiClient.ExecuteQueryAsync<ListProjetoViewModel>("Projeto", "ListProjeto", new ListProjetoQuery(true));

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return Page();
            }

            Lista = result.Projetos;

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
