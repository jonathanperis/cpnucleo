using Cpnucleo.Shared.Commands.UpdateRecurso;
using Cpnucleo.Shared.Queries.GetRecurso;

namespace Cpnucleo.RazorPages.Pages.Recurso;

[Authorize]
public class AlterarModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public AlterarModel(ICpnucleoApiClient cpnucleoApiClient)
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

            var result = await _cpnucleoApiClient.ExecuteAsync<OperationResult>("Recurso", "UpdateRecurso", new UpdateRecursoCommand(Recurso.Id, Recurso.Nome, Recurso.Senha));

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
        var result = await _cpnucleoApiClient.ExecuteAsync<GetRecursoViewModel>("Recurso", "GetRecurso", new GetRecursoQuery(idRecurso));

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        Recurso = result.Recurso;
    }
}
