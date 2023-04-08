using Cpnucleo.Shared.Commands.UpdateSistema;
using Cpnucleo.Shared.Queries.GetSistema;

namespace Cpnucleo.RazorPages.Pages.Sistema;

[Authorize]
public class AlterarModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public AlterarModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public SistemaDTO Sistema { get; set; }

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
                await CarregarDados(Sistema.Id);

                return Page();
            }

            OperationResult result = await _cpnucleoApiClient.ExecuteAsync<OperationResult>("Sistema", "UpdateSistema", new UpdateSistemaCommand(Sistema.Id, Sistema.Nome, Sistema.Descricao));

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

    private async Task CarregarDados(Guid idSistema)
    {
        GetSistemaViewModel result = await _cpnucleoApiClient.ExecuteAsync<GetSistemaViewModel>("Sistema", "GetSistema", new GetSistemaQuery(idSistema));

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        Sistema = result.Sistema;
    }
}
