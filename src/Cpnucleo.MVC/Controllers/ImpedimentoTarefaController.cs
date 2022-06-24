namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class ImpedimentoTarefaController : BaseController
{
    private readonly IImpedimentoTarefaGrpcService _impedimentoTarefaGrpcService;
    private readonly ITarefaGrpcService _tarefaGrpcService;
    private readonly IImpedimentoGrpcService _impedimentoGrpcService;

    private ImpedimentoTarefaViewModel _viewModel;

    public ImpedimentoTarefaController(IConfiguration configuration)
        : base(configuration)
    {
        _impedimentoTarefaGrpcService = MagicOnionClient.Create<IImpedimentoTarefaGrpcService>(CreateAuthenticatedChannel());
        _tarefaGrpcService = MagicOnionClient.Create<ITarefaGrpcService>(CreateAuthenticatedChannel());
        _impedimentoGrpcService = MagicOnionClient.Create<IImpedimentoGrpcService>(CreateAuthenticatedChannel());
    }

    public ImpedimentoTarefaViewModel ViewModel
    {
        get
        {
            if (_viewModel == null)
            {
                _viewModel = new ImpedimentoTarefaViewModel();
            }

            return _viewModel;
        }
        set => _viewModel = value;
    }

    [HttpGet]
    public async Task<IActionResult> Listar(Guid idTarefa)
    {
        try
        {
            GetImpedimentoTarefaByTarefaViewModel result = await _impedimentoTarefaGrpcService.GetImpedimentoTarefaByTarefa(new GetImpedimentoTarefaByTarefaQuery(idTarefa));

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            ViewModel.Lista = result.ImpedimentoTarefas;

            ViewData["idTarefa"] = idTarefa;

            return View(ViewModel);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Incluir(Guid idTarefa)
    {
        await CarregarDados(null, idTarefa);

        return View(ViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Incluir(ImpedimentoTarefaViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(null, obj.Tarefa.Id);

                return View(ViewModel);
            }

            OperationResult result = await _impedimentoTarefaGrpcService.CreateImpedimentoTarefa(new CreateImpedimentoTarefaCommand(Guid.Empty, obj.ImpedimentoTarefa.Descricao, obj.ImpedimentoTarefa.IdTarefa, obj.ImpedimentoTarefa.IdImpedimento));

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            return RedirectToAction("Listar", new { idTarefa = obj.ImpedimentoTarefa.IdTarefa });
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
    public async Task<IActionResult> Alterar(ImpedimentoTarefaViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(obj.ImpedimentoTarefa.Id);

                return View(ViewModel);
            }

            OperationResult result = await _impedimentoTarefaGrpcService.UpdateImpedimentoTarefa(new UpdateImpedimentoTarefaCommand(obj.ImpedimentoTarefa.Id, obj.ImpedimentoTarefa.Descricao, obj.ImpedimentoTarefa.IdTarefa, obj.ImpedimentoTarefa.IdImpedimento));

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            return RedirectToAction("Listar", new { idTarefa = obj.ImpedimentoTarefa.IdTarefa });
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
    public async Task<IActionResult> Remover(ImpedimentoTarefaViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(obj.ImpedimentoTarefa.Id);

                return View(ViewModel);
            }

            OperationResult result = await _impedimentoTarefaGrpcService.RemoveImpedimentoTarefa(new RemoveImpedimentoTarefaCommand(obj.ImpedimentoTarefa.Id));

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            return RedirectToAction("Listar", new { idTarefa = obj.ImpedimentoTarefa.IdTarefa });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    private async Task CarregarDados(Guid? idImpedimentoTarefa = default, Guid? idTarefa = default)
    {
        if (idImpedimentoTarefa is not null)
        {
            GetImpedimentoTarefaViewModel result = await _impedimentoTarefaGrpcService.GetImpedimentoTarefa(new GetImpedimentoTarefaQuery(idImpedimentoTarefa.Value));

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return;
            }

            ViewModel.ImpedimentoTarefa = result.ImpedimentoTarefa;
        }

        if (idTarefa is not null)
        {
            GetTarefaViewModel result = await _tarefaGrpcService.GetTarefa(new GetTarefaQuery(idTarefa.Value));

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return;
            }

            ViewModel.Tarefa = result.Tarefa;
        }

        ListImpedimentoViewModel result2 = await _impedimentoGrpcService.ListImpedimento(new ListImpedimentoQuery());

        if (result2.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        ViewModel.SelectImpedimentos = new SelectList(result2.Impedimentos, "Id", "Nome");
    }
}
