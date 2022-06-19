namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class RecursoController : BaseController
{
    private readonly IRecursoGrpcService _recursoGrpcService;

    private RecursoViewModel _viewModel;

    public RecursoController(IConfiguration configuration)
        : base(configuration)
    {
        _recursoGrpcService = MagicOnionClient.Create<IRecursoGrpcService>(CreateAuthenticatedChannel());
    }

    public RecursoViewModel ViewModel
    {
        get
        {
            if (_viewModel == null)
            {
                _viewModel = new RecursoViewModel();
            }

            return _viewModel;
        }
        set => _viewModel = value;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        try
        {
            ListRecursoViewModel result = await _recursoGrpcService.ListRecurso(new ListRecursoQuery { });

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            ViewModel.Lista = result.Recursos;

            return View(ViewModel);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpGet]
    public IActionResult Incluir()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Incluir(RecursoViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            OperationResult result = await _recursoGrpcService.CreateRecurso(new CreateRecursoCommand(Guid.Empty, obj.Recurso.Nome, obj.Recurso.Login, obj.Recurso.Senha));

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            return RedirectToAction("Listar");
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
    public async Task<IActionResult> Alterar(RecursoViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(obj.Recurso.Id);

                return View(ViewModel);
            }

            OperationResult result = await _recursoGrpcService.UpdateRecurso(new UpdateRecursoCommand(obj.Recurso.Id, obj.Recurso.Nome, obj.Recurso.Senha));

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            return RedirectToAction("Listar");
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
    public async Task<IActionResult> Remover(RecursoViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(obj.Recurso.Id);

                return View(ViewModel);
            }

            OperationResult result = await _recursoGrpcService.RemoveRecurso(new RemoveRecursoCommand(obj.Recurso.Id));

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            return RedirectToAction("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    private async Task CarregarDados(Guid id)
    {
        GetRecursoViewModel result = await _recursoGrpcService.GetRecurso(new GetRecursoQuery { Id = id });

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        ViewModel.Recurso = result.Recurso;
    }
}
