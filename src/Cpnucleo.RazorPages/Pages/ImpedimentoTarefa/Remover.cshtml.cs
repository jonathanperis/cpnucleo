namespace Cpnucleo.RazorPages.Pages.ImpedimentoTarefa;

[Authorize]
public class RemoverModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public RemoverModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public ImpedimentoTarefaDTO ImpedimentoTarefa { get; set; }

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
                await CarregarDados(ImpedimentoTarefa.Id);

                return Page();
            }

            OperationResult result = await _cpnucleoApiClient.ExecuteCommandAsync<OperationResult>("ImpedimentoTarefa", "RemoveImpedimentoTarefa", new RemoveImpedimentoTarefaCommand { Id = ImpedimentoTarefa.Id });

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return Page();
            }

            return RedirectToPage("Listar", new { idTarefa = ImpedimentoTarefa.IdTarefa });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    private async Task CarregarDados(Guid idImpedimentoTarefa)
    {
        GetImpedimentoTarefaViewModel result = await _cpnucleoApiClient.ExecuteQueryAsync<GetImpedimentoTarefaViewModel>("ImpedimentoTarefa", "GetImpedimentoTarefa", new GetImpedimentoTarefaQuery { Id = idImpedimentoTarefa });

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        ImpedimentoTarefa = result.ImpedimentoTarefa;
    }
}
