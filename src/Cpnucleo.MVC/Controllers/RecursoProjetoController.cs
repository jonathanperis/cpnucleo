namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class RecursoProjetoController : BaseController
{
    private readonly IRecursoProjetoGrpcService _recursoProjetoGrpcService;
    private readonly IRecursoGrpcService _recursoGrpcService;
    private readonly IProjetoGrpcService _projetoGrpcService;

    private RecursoProjetoViewModel _viewModel;

    public RecursoProjetoController(IConfiguration configuration)
        : base(configuration)
    {
        _recursoProjetoGrpcService = MagicOnionClient.Create<IRecursoProjetoGrpcService>(CreateAuthenticatedChannel());
        _recursoGrpcService = MagicOnionClient.Create<IRecursoGrpcService>(CreateAuthenticatedChannel());
        _projetoGrpcService = MagicOnionClient.Create<IProjetoGrpcService>(CreateAuthenticatedChannel());
    }

    public RecursoProjetoViewModel ViewModel
    {
        get
        {
            if (_viewModel == null)
            {
                _viewModel = new RecursoProjetoViewModel();
            }

            return _viewModel;
        }
        set => _viewModel = value;
    }

    [HttpGet]
    public async Task<IActionResult> Listar(Guid idProjeto)
    {
        try
        {
            GetRecursoProjetoByProjetoViewModel result = await _recursoProjetoGrpcService.GetRecursoProjetoByProjeto(new GetRecursoProjetoByProjetoQuery { IdProjeto = idProjeto });

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            ViewModel.Lista = result.RecursoProjetos;

            ViewData["idProjeto"] = idProjeto;

            return View(ViewModel);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Incluir(Guid idProjeto)
    {
        await CarregarDados(null, idProjeto);

        return View(ViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Incluir(RecursoProjetoViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(null, obj.Projeto.Id);

                return View(ViewModel);
            }

            OperationResult result = await _recursoProjetoGrpcService.CreateRecursoProjeto(new CreateRecursoProjetoCommand { IdProjeto = obj.RecursoProjeto.IdProjeto, IdRecurso = obj.RecursoProjeto.IdRecurso });

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            return RedirectToAction("Listar", new { idProjeto = obj.RecursoProjeto.IdProjeto });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Alterar(Guid id)
    {
        try
        {
            await CarregarDados(id);

            return View(ViewModel);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Alterar(RecursoProjetoViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(obj.RecursoProjeto.Id);

                return View(ViewModel);
            }

            OperationResult result = await _recursoProjetoGrpcService.UpdateRecursoProjeto(new UpdateRecursoProjetoCommand { Id = obj.RecursoProjeto.Id, IdProjeto = obj.RecursoProjeto.IdProjeto, IdRecurso = obj.RecursoProjeto.IdRecurso });

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            return RedirectToAction("Listar", new { idProjeto = obj.RecursoProjeto.IdProjeto });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Remover(Guid id)
    {
        try
        {
            await CarregarDados(id);

            return View(ViewModel);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Remover(RecursoProjetoViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(obj.RecursoProjeto.Id);

                return View(ViewModel);
            }

            OperationResult result = await _recursoProjetoGrpcService.RemoveRecursoProjeto(new RemoveRecursoProjetoCommand { Id = obj.RecursoProjeto.Id });

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            return RedirectToAction("Listar", new { idProjeto = obj.RecursoProjeto.IdProjeto });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    private async Task CarregarDados(Guid? idRecursoProjeto = default, Guid? idProjeto = default)
    {
        if (idRecursoProjeto is not null)
        {
            GetRecursoProjetoViewModel result = await _recursoProjetoGrpcService.GetRecursoProjeto(new GetRecursoProjetoQuery { Id = idRecursoProjeto.Value });

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return;
            }

            ViewModel.RecursoProjeto = result.RecursoProjeto;
        }

        if (idProjeto is not null)
        {
            GetProjetoViewModel result = await _projetoGrpcService.GetProjeto(new GetProjetoQuery { Id = idProjeto.Value });

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return;
            }

            ViewModel.Projeto = result.Projeto;
        }

        ListRecursoViewModel result2 = await _recursoGrpcService.ListRecurso(new ListRecursoQuery { });

        if (result2.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        ViewModel.SelectRecursos = new SelectList(result2.Recursos, "Id", "Nome");
    }
}
