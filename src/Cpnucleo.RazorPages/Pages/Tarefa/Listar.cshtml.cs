﻿namespace Cpnucleo.RazorPages.Pages.Tarefa;

//[Authorize]
public class ListarModel : PageBase
{
    private readonly ICpnucleoApiClient _cpnucleoApiClient;

    public ListarModel(ICpnucleoApiClient cpnucleoApiClient)
    {
        _cpnucleoApiClient = cpnucleoApiClient;
    }

    public TarefaDTO Tarefa { get; set; }

    public IEnumerable<TarefaDTO> Lista { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Lista = await _cpnucleoApiClient.GetAsync<IEnumerable<TarefaDTO>>("tarefa", Token, true);

            return Page();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
