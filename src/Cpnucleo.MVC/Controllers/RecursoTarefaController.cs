namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class RecursoTarefaController : BaseController
{
    private readonly IRecursoTarefaGrpcService _recursoTarefaGrpcService;
    private readonly IRecursoGrpcService _recursoGrpcService;
    private readonly ITarefaGrpcService _tarefaGrpcService;

    private RecursoTarefaViewModel _viewModel;

    public RecursoTarefaController(IConfiguration configuration)
        : base(configuration)
    {
        _recursoTarefaGrpcService = MagicOnionClient.Create<IRecursoTarefaGrpcService>(CreateAuthenticatedChannel());
        _recursoGrpcService = MagicOnionClient.Create<IRecursoGrpcService>(CreateAuthenticatedChannel());
        _tarefaGrpcService = MagicOnionClient.Create<ITarefaGrpcService>(CreateAuthenticatedChannel());
    }

    public RecursoTarefaViewModel ViewModel
    {
        get
        {
            if (_viewModel == null)
            {
                _viewModel = new RecursoTarefaViewModel();
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
            GetRecursoTarefaByTarefaViewModel result = await _recursoTarefaGrpcService.GetRecursoTarefaByTarefa(new GetRecursoTarefaByTarefaQuery(idTarefa));

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            ViewModel.Lista = result.RecursoTarefas;

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
    public async Task<IActionResult> Incluir(RecursoTarefaViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(null, obj.Tarefa.Id);

                return View(ViewModel);
            }

            OperationResult result = await _recursoTarefaGrpcService.CreateRecursoTarefa(new CreateRecursoTarefaCommand(Guid.Empty, obj.RecursoTarefa.IdRecurso, obj.RecursoTarefa.IdTarefa));

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            return RedirectToAction("Listar", new { idTarefa = obj.RecursoTarefa.IdTarefa });
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
    public async Task<IActionResult> Remover(RecursoTarefaViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(obj.RecursoTarefa.Id);

                return View(ViewModel);
            }

            OperationResult result = await _recursoTarefaGrpcService.RemoveRecursoTarefa(new RemoveRecursoTarefaCommand(obj.RecursoTarefa.Id));

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            return RedirectToAction("Listar", new { idTarefa = obj.RecursoTarefa.IdTarefa });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    private async Task CarregarDados(Guid? idRecursoTarefa = default, Guid? idTarefa = default)
    {
        if (idRecursoTarefa is not null)
        {
            GetRecursoTarefaViewModel result = await _recursoTarefaGrpcService.GetRecursoTarefa(new GetRecursoTarefaQuery(idRecursoTarefa.Value));

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return;
            }

            ViewModel.RecursoTarefa = result.RecursoTarefa;
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

        ListRecursoViewModel result2 = await _recursoGrpcService.ListRecurso(new ListRecursoQuery());

        if (result2.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        ViewModel.SelectRecursos = new SelectList(result2.Recursos, "Id", "Nome");
    }
}
