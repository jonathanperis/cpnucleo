﻿namespace Cpnucleo.RazorPages.Pages.ImpedimentoTarefa;

//[Authorize]
public class IncluirModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public IncluirModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public ImpedimentoTarefaDTO ImpedimentoTarefa { get; set; }

    public TarefaDTO Tarefa { get; set; }

    public SelectList SelectImpedimentos { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid idTarefa)
    {
        try
        {
            await CarregarDados(idTarefa);

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
                await CarregarDados(ImpedimentoTarefa.IdTarefa);

                return Page();
            }

            var result = await _cpnucleoApiClient.ExecuteCommandAsync<OperationResult>("ImpedimentoTarefa", "CreateImpedimentoTarefa", Token, new CreateImpedimentoTarefaCommand { Descricao = ImpedimentoTarefa.Descricao, IdImpedimento = ImpedimentoTarefa.IdImpedimento, IdTarefa = ImpedimentoTarefa.IdTarefa });

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

    private async Task CarregarDados(Guid idTarefa)
    {
        var result = await _cpnucleoApiClient.ExecuteQueryAsync<GetTarefaViewModel>("Tarefa", "GetTarefa", Token, new GetTarefaQuery { Id = idTarefa });

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        Tarefa = result.Tarefa;

        var result2 = await _cpnucleoApiClient.ExecuteQueryAsync<ListImpedimentoViewModel>("Impedimento", "ListImpedimento", Token, new ListImpedimentoQuery { });

        if (result2.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        SelectImpedimentos = new SelectList(result2.Impedimentos, "Id", "Nome");
    }
}
