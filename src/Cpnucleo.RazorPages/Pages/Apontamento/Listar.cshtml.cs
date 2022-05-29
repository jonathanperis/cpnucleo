using Cpnucleo.RazorPages.Services;
using System.Security.Claims;

namespace Cpnucleo.RazorPages.Pages.Apontamento;

[Authorize]
public class ListarModel : PageBase
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
            string retorno = ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
            Guid idRecurso = new(retorno);

            Lista = await _cpnucleoApiClient.GetAsync<IEnumerable<ApontamentoDTO>>("apontamento", "getbyrecurso", Token, idRecurso);
            ListaTarefas = await _cpnucleoApiClient.GetAsync<IEnumerable<TarefaDTO>>("tarefa", "getbyrecurso", Token, idRecurso);

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
                string retorno = ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
                Guid idRecurso = new(retorno);

                Lista = await _cpnucleoApiClient.GetAsync<IEnumerable<ApontamentoDTO>>("apontamento", "getbyrecurso", Token, idRecurso);
                ListaTarefas = await _cpnucleoApiClient.GetAsync<IEnumerable<TarefaDTO>>("tarefa", "getbyrecurso", Token, idRecurso);

                return Page();
            }

            await _cpnucleoApiClient.PostAsync<ApontamentoDTO>("apontamento", Token, Apontamento);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
