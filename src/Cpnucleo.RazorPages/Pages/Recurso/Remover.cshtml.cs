using Cpnucleo.Shared.Commands.RemoveRecurso;
using Cpnucleo.Shared.Queries.GetRecurso;

namespace Cpnucleo.RazorPages.Pages.Recurso;

[Authorize]
public class RemoverModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public RemoverModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public RecursoDTO Recurso { get; set; }

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
                await CarregarDados(Recurso.Id);

                return Page();
            }

            OperationResult result = await _cpnucleoApiClient.ExecuteAsync<OperationResult>("Recurso", "RemoveRecurso", new RemoveRecursoCommand(Recurso.Id));

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return Page();
            }

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    private async Task CarregarDados(Guid idRecurso)
    {
        GetRecursoViewModel result = await _cpnucleoApiClient.ExecuteAsync<GetRecursoViewModel>("Recurso", "GetRecurso", new GetRecursoQuery(idRecurso));

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        Recurso = result.Recurso;
    }
}
