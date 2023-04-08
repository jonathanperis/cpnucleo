using Cpnucleo.Shared.Commands.RemoveImpedimento;
using Cpnucleo.Shared.Queries.GetImpedimento;

namespace Cpnucleo.RazorPages.Pages.Impedimento;

[Authorize]
public class RemoverModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public RemoverModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public ImpedimentoDTO Impedimento { get; set; }

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
                await CarregarDados(Impedimento.Id);

                return Page();
            }

            OperationResult result = await _cpnucleoApiClient.ExecuteAsync<OperationResult>("Impedimento", "RemoveImpedimento", new RemoveImpedimentoCommand(Impedimento.Id));

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

    private async Task CarregarDados(Guid idImpedimento)
    {
        GetImpedimentoViewModel result = await _cpnucleoApiClient.ExecuteAsync<GetImpedimentoViewModel>("Impedimento", "GetImpedimento", new GetImpedimentoQuery(idImpedimento));

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        Impedimento = result.Impedimento;
    }
}
