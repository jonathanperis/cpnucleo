using Cpnucleo.Shared.Commands.RemoveProjeto;
using Cpnucleo.Shared.Queries.GetProjeto;

namespace Cpnucleo.RazorPages.Pages.Projeto;

[Authorize]
public class RemoverModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public RemoverModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public ProjetoDTO Projeto { get; set; }

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
                await CarregarDados(Projeto.Id);

                return Page();
            }

            OperationResult result = await _cpnucleoApiClient.ExecuteAsync<OperationResult>("Projeto", "RemoveProjeto", new RemoveProjetoCommand(Projeto.Id));

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

    private async Task CarregarDados(Guid idProjeto)
    {
        GetProjetoViewModel result = await _cpnucleoApiClient.ExecuteAsync<GetProjetoViewModel>("Projeto", "GetProjeto", new GetProjetoQuery(idProjeto));

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        Projeto = result.Projeto;
    }
}
