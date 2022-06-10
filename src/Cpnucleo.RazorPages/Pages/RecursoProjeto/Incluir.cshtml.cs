namespace Cpnucleo.RazorPages.Pages.RecursoProjeto;

[Authorize]
public class IncluirModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public IncluirModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public RecursoProjetoDTO RecursoProjeto { get; set; }

    public ProjetoDTO Projeto { get; set; }

    public SelectList SelectRecursos { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid idProjeto)
    {
        try
        {
            await CarregarDados(idProjeto);

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(RecursoProjeto.IdProjeto);

                return Page();
            }

            OperationResult result = await _cpnucleoApiClient.ExecuteCommandAsync<OperationResult>("RecursoProjeto", "CreateRecursoProjeto", new CreateRecursoProjetoCommand { IdProjeto = RecursoProjeto.IdProjeto, IdRecurso = RecursoProjeto.IdRecurso });

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return Page();
            }

            return RedirectToPage("Listar", new { idProjeto = RecursoProjeto.IdProjeto });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    private async Task CarregarDados(Guid idProjeto)
    {
        GetProjetoViewModel result = await _cpnucleoApiClient.ExecuteQueryAsync<GetProjetoViewModel>("Projeto", "GetProjeto", new GetProjetoQuery { Id = idProjeto });

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        Projeto = result.Projeto;

        ListRecursoViewModel result2 = await _cpnucleoApiClient.ExecuteQueryAsync<ListRecursoViewModel>("Recurso", "ListRecurso", new ListRecursoQuery { });

        if (result2.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        SelectRecursos = new SelectList(result2.Recursos, "Id", "Nome");
    }
}
