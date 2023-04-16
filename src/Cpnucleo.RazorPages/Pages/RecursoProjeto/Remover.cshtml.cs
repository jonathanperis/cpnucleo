using Cpnucleo.Shared.Commands.RemoveRecursoProjeto;
using Cpnucleo.Shared.Queries.GetRecursoProjeto;

namespace Cpnucleo.RazorPages.Pages.RecursoProjeto;

[Authorize]
public class RemoverModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public RemoverModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public RecursoProjetoDto RecursoProjeto { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        try
        {
            await CarregarDados(id);

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
                await CarregarDados(RecursoProjeto.Id);

                return Page();
            }

            var result = await _cpnucleoApiClient.ExecuteAsync<OperationResult>("RecursoProjeto", "RemoveRecursoProjeto", new RemoveRecursoProjetoCommand(RecursoProjeto.Id));

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

    private async Task CarregarDados(Guid idRecursoProjeto)
    {
        var result = await _cpnucleoApiClient.ExecuteAsync<GetRecursoProjetoViewModel>("RecursoProjeto", "GetRecursoProjeto", new GetRecursoProjetoQuery(idRecursoProjeto));

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        RecursoProjeto = result.RecursoProjeto;
    }
}
