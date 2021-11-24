using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa;

namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class ImpedimentoTarefaController : BaseController
{
    private readonly IImpedimentoTarefaGrpcService _impedimentoTarefaGrpcService;
    private readonly ITarefaGrpcService _tarefaGrpcService;
    private readonly IImpedimentoGrpcService _impedimentoGrpcService;

    private ImpedimentoTarefaView _impedimentoTarefaView;

    public ImpedimentoTarefaController(IConfiguration configuration)
        : base(configuration)
    {
        _impedimentoTarefaGrpcService = MagicOnionClient.Create<IImpedimentoTarefaGrpcService>(CreateAuthenticatedChannel());
        _tarefaGrpcService = MagicOnionClient.Create<ITarefaGrpcService>(CreateAuthenticatedChannel());
        _impedimentoGrpcService = MagicOnionClient.Create<IImpedimentoGrpcService>(CreateAuthenticatedChannel());
    }

    public ImpedimentoTarefaView ImpedimentoTarefaView
    {
        get
        {
            if (_impedimentoTarefaView == null)
            {
                _impedimentoTarefaView = new ImpedimentoTarefaView();
            }

            return _impedimentoTarefaView;
        }
        set => _impedimentoTarefaView = value;
    }

    [HttpGet]
    public async Task<IActionResult> Listar(Guid idTarefa)
    {
        try
        {
            ImpedimentoTarefaView.Lista = await _impedimentoTarefaGrpcService.GetByTarefaAsync(new GetByTarefaQuery { IdTarefa = idTarefa });

            ViewData["idTarefa"] = idTarefa;

            return View(ImpedimentoTarefaView);
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
        await CarregarSelectImpedimentos();

        return View(ImpedimentoTarefaView);
    }

    [HttpPost]
    public async Task<IActionResult> Incluir(ImpedimentoTarefaView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await ObterTarefa(obj.Tarefa.Id);
                await CarregarSelectImpedimentos();

                return View(ImpedimentoTarefaView);
            }

            await _impedimentoTarefaGrpcService.AddAsync(new CreateImpedimentoTarefaCommand { ImpedimentoTarefa = obj.ImpedimentoTarefa });

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
            ImpedimentoTarefaView.ImpedimentoTarefa = await _impedimentoTarefaGrpcService.GetAsync(new GetImpedimentoTarefaQuery { Id = id });

            await CarregarSelectImpedimentos();

            return View(ImpedimentoTarefaView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Alterar(ImpedimentoTarefaView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ImpedimentoTarefaView.ImpedimentoTarefa = await _impedimentoTarefaGrpcService.GetAsync(new GetImpedimentoTarefaQuery { Id = obj.ImpedimentoTarefa.Id });

                await CarregarSelectImpedimentos();

                return View(ImpedimentoTarefaView);
            }

            await _impedimentoTarefaGrpcService.UpdateAsync(new UpdateImpedimentoTarefaCommand { ImpedimentoTarefa = obj.ImpedimentoTarefa });

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
            ImpedimentoTarefaView.ImpedimentoTarefa = await _impedimentoTarefaGrpcService.GetAsync(new GetImpedimentoTarefaQuery { Id = id });

            return View(ImpedimentoTarefaView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Remover(ImpedimentoTarefaView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ImpedimentoTarefaView.ImpedimentoTarefa = await _impedimentoTarefaGrpcService.GetAsync(new GetImpedimentoTarefaQuery { Id = obj.ImpedimentoTarefa.Id });

                return View(ImpedimentoTarefaView);
            }

            await _impedimentoTarefaGrpcService.RemoveAsync(new RemoveImpedimentoTarefaCommand { Id = obj.ImpedimentoTarefa.Id });

            return RedirectToAction("Listar", new { idTarefa = obj.ImpedimentoTarefa.IdTarefa });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    private async Task ObterTarefa(Guid idTarefa)
    {
        ImpedimentoTarefaView.Tarefa = await _tarefaGrpcService.GetAsync(new GetTarefaQuery { Id = idTarefa });
    }

    private async Task CarregarSelectImpedimentos()
    {
        IEnumerable<ImpedimentoViewModel> response = await _impedimentoGrpcService.AllAsync(new ListImpedimentoQuery { });
        ImpedimentoTarefaView.SelectImpedimentos = new SelectList(response, "Id", "Nome");
    }
}
