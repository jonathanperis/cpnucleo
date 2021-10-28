using Cpnucleo.RazorPages.Services;
using System.Security.Claims;

namespace Cpnucleo.RazorPages.Pages.Apontamento;

[Authorize]
public class ListarModel : PageBase
{
    private readonly ICpnucleoApiService _cpnucleoApiService;

    public ListarModel(ICpnucleoApiService cpnucleoApiService)
    {
        _cpnucleoApiService = cpnucleoApiService;
    }

    [BindProperty]
    public ApontamentoViewModel Apontamento { get; set; }

    public IEnumerable<ApontamentoViewModel> Lista { get; set; }

    public IEnumerable<TarefaViewModel> ListaTarefas { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            string retorno = ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
            Guid idRecurso = new(retorno);

            Lista = await _cpnucleoApiService.GetAsync<IEnumerable<ApontamentoViewModel>>("apontamento", "getbyrecurso", Token, idRecurso);
            ListaTarefas = await _cpnucleoApiService.GetAsync<IEnumerable<TarefaViewModel>>("tarefa", "getbyrecurso", Token, idRecurso);

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

                Lista = await _cpnucleoApiService.GetAsync<IEnumerable<ApontamentoViewModel>>("apontamento", "getbyrecurso", Token, idRecurso);
                ListaTarefas = await _cpnucleoApiService.GetAsync<IEnumerable<TarefaViewModel>>("tarefa", "getbyrecurso", Token, idRecurso);

                return Page();
            }

            await _cpnucleoApiService.PostAsync<ApontamentoViewModel>("apontamento", Token, Apontamento);

            return RedirectToPage("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
