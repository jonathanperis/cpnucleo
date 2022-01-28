using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa;

namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class RecursoTarefaController : BaseController
{
    private readonly IRecursoTarefaGrpcService _recursoTarefaGrpcService;
    private readonly IRecursoGrpcService _recursoGrpcService;
    private readonly ITarefaGrpcService _TarefaGrpcService;

    private RecursoTarefaView _recursoTarefaView;

    public RecursoTarefaController(IConfiguration configuration)
        : base(configuration)
    {
        _recursoTarefaGrpcService = MagicOnionClient.Create<IRecursoTarefaGrpcService>(CreateAuthenticatedChannel());
        _recursoGrpcService = MagicOnionClient.Create<IRecursoGrpcService>(CreateAuthenticatedChannel());
        _TarefaGrpcService = MagicOnionClient.Create<ITarefaGrpcService>(CreateAuthenticatedChannel());
    }

    public RecursoTarefaView RecursoTarefaView
    {
        get
        {
            if (_recursoTarefaView == null)
            {
                _recursoTarefaView = new RecursoTarefaView();
            }

            return _recursoTarefaView;
        }
        set => _recursoTarefaView = value;
    }

    [HttpGet]
    public async Task<IActionResult> Listar(Guid idTarefa)
    {
        try
        {
            RecursoTarefaView.Lista = await _recursoTarefaGrpcService.GetByTarefaAsync(new GetByTarefaQuery { IdTarefa = idTarefa });

            ViewData["idTarefa"] = idTarefa;

            return View(RecursoTarefaView);
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
        await ObterTarefa(idTarefa);
        await CarregarSelectRecursos();

        return View(RecursoTarefaView);
    }

    [HttpPost]
    public async Task<IActionResult> Incluir(RecursoTarefaView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await ObterTarefa(obj.Tarefa.Id);
                await CarregarSelectRecursos();

                return View(RecursoTarefaView);
            }

            await _recursoTarefaGrpcService.AddAsync(new CreateRecursoTarefaCommand { RecursoTarefa = obj.RecursoTarefa });

            return RedirectToAction("Listar", new { idTarefa = obj.RecursoTarefa.IdTarefa });
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
            RecursoTarefaView.RecursoTarefa = await _recursoTarefaGrpcService.GetAsync(new GetRecursoTarefaQuery { Id = id });

            await CarregarSelectRecursos();

            return View(RecursoTarefaView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Alterar(RecursoTarefaView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                RecursoTarefaView.RecursoTarefa = await _recursoTarefaGrpcService.GetAsync(new GetRecursoTarefaQuery { Id = obj.RecursoTarefa.Id });

                await CarregarSelectRecursos();

                return View(RecursoTarefaView);
            }

            await _recursoTarefaGrpcService.UpdateAsync(new UpdateRecursoTarefaCommand { RecursoTarefa = obj.RecursoTarefa });

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
            RecursoTarefaView.RecursoTarefa = await _recursoTarefaGrpcService.GetAsync(new GetRecursoTarefaQuery { Id = id });

            return View(RecursoTarefaView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Remover(RecursoTarefaView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                RecursoTarefaView.RecursoTarefa = await _recursoTarefaGrpcService.GetAsync(new GetRecursoTarefaQuery { Id = obj.RecursoTarefa.Id });

                return View(RecursoTarefaView);
            }

            await _recursoTarefaGrpcService.RemoveAsync(new RemoveRecursoTarefaCommand { Id = obj.RecursoTarefa.Id });

            return RedirectToAction("Listar", new { idTarefa = obj.RecursoTarefa.IdTarefa });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    private async Task ObterTarefa(Guid idTarefa)
    {
        RecursoTarefaView.Tarefa = await _TarefaGrpcService.GetAsync(new GetTarefaQuery { Id = idTarefa });
    }

    private async Task CarregarSelectRecursos()
    {
        IEnumerable<RecursoViewModel> response = await _recursoGrpcService.AllAsync(new ListRecursoQuery { });
        RecursoTarefaView.SelectRecursos = new SelectList(response, "Id", "Nome");
    }
}
