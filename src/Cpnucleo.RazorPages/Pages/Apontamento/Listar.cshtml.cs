using Cpnucleo.RazorPages.Services;
using Cpnucleo.Shared.Common.DTOs;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.Apontamento;
using Cpnucleo.Shared.Queries.Tarefa;
using System.Security.Claims;

namespace Cpnucleo.RazorPages.Pages.Apontamento;

[Authorize]
public class ListarModel : PageModel
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public ListarModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    [BindProperty]
    public ApontamentoDTO Apontamento { get; set; }

    public IEnumerable<ApontamentoDTO> Lista { get; set; }

    public IEnumerable<TarefaDTO> ListaTarefas { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            await CarregarDados();

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
                await CarregarDados();

                return Page();
            }

            OperationResult result = await _cpnucleoApiClient.ExecuteCommandAsync<OperationResult>("Apontamento", "CreateApontamento", new CreateApontamentoCommand(Guid.Empty, Apontamento.Descricao, Apontamento.DataApontamento, Apontamento.QtdHoras, Apontamento.IdTarefa, Apontamento.IdRecurso));

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

    private async Task CarregarDados()
    {
        string retorno = ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
        Guid idRecurso = new(retorno);

        GetApontamentoByRecursoViewModel result = await _cpnucleoApiClient.ExecuteQueryAsync<GetApontamentoByRecursoViewModel>("Apontamento", "GetApontamentoByRecurso", new GetApontamentoByRecursoQuery(idRecurso));

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        Lista = result.Apontamentos;

        GetTarefaByRecursoViewModel result2 = await _cpnucleoApiClient.ExecuteQueryAsync<GetTarefaByRecursoViewModel>("Tarefa", "GetTarefaByRecurso", new GetTarefaByRecursoQuery(idRecurso));

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        ListaTarefas = result2.Tarefas;
    }
}
