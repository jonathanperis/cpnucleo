using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow;

namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class TarefaController : BaseController
{
    private readonly ITarefaGrpcService _tarefaGrpcService;
    private readonly ISistemaGrpcService _sistemaGrpcService;
    private readonly IProjetoGrpcService _projetoGrpcService;
    private readonly IWorkflowGrpcService _workflowGrpcService;
    private readonly ITipoTarefaGrpcService _tipoTarefaGrpcService;

    private TarefaView _tarefaView;

    public TarefaController(IConfiguration configuration)
        : base(configuration)
    {
        _tarefaGrpcService = MagicOnionClient.Create<ITarefaGrpcService>(CreateAuthenticatedChannel());
        _sistemaGrpcService = MagicOnionClient.Create<ISistemaGrpcService>(CreateAuthenticatedChannel());
        _projetoGrpcService = MagicOnionClient.Create<IProjetoGrpcService>(CreateAuthenticatedChannel());
        _workflowGrpcService = MagicOnionClient.Create<IWorkflowGrpcService>(CreateAuthenticatedChannel());
        _tipoTarefaGrpcService = MagicOnionClient.Create<ITipoTarefaGrpcService>(CreateAuthenticatedChannel());
    }

    public TarefaView TarefaView
    {
        get
        {
            if (_tarefaView == null)
            {
                _tarefaView = new TarefaView();
            }

            return _tarefaView;
        }
        set => _tarefaView = value;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        try
        {
            TarefaView.Lista = await _tarefaGrpcService.AllAsync(new ListTarefaQuery { GetDependencies = true });

            return View(TarefaView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Incluir()
    {
        await CarregarSelectSistemas();
        await CarregarSelectProjetos();
        await CarregarSelectWorkflows();
        await CarregarSelectTipoTarefas();

        TarefaView.User = HttpContext.User;

        return View(TarefaView);
    }

    [HttpPost]
    public async Task<IActionResult> Incluir(TarefaView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarSelectSistemas();
                await CarregarSelectProjetos();
                await CarregarSelectWorkflows();
                await CarregarSelectTipoTarefas();

                TarefaView.User = HttpContext.User;

                return View(TarefaView);
            }

            await _tarefaGrpcService.AddAsync(new CreateTarefaCommand { Tarefa = obj.Tarefa });

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
            TarefaView.Tarefa = await _tarefaGrpcService.GetAsync(new GetTarefaQuery { Id = id });

            await CarregarSelectSistemas();
            await CarregarSelectProjetos();
            await CarregarSelectWorkflows();
            await CarregarSelectTipoTarefas();

            return View(TarefaView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Alterar(TarefaView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TarefaView.Tarefa = await _tarefaGrpcService.GetAsync(new GetTarefaQuery { Id = obj.Tarefa.Id });

                await CarregarSelectSistemas();
                await CarregarSelectProjetos();
                await CarregarSelectWorkflows();
                await CarregarSelectTipoTarefas();

                return View(TarefaView);
            }

            await _tarefaGrpcService.UpdateAsync(new UpdateTarefaCommand { Tarefa = obj.Tarefa });

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
            TarefaView.Tarefa = await _tarefaGrpcService.GetAsync(new GetTarefaQuery { Id = id });

            return View(TarefaView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Remover(TarefaView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TarefaView.Tarefa = await _tarefaGrpcService.GetAsync(new GetTarefaQuery { Id = obj.Tarefa.Id });

                return View(TarefaView);
            }

            await _tarefaGrpcService.RemoveAsync(new RemoveTarefaCommand { Id = obj.Tarefa.Id });

            return RedirectToAction("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    private async Task CarregarSelectSistemas()
    {
        IEnumerable<SistemaViewModel> response = await _sistemaGrpcService.AllAsync(new ListSistemaQuery { });
        TarefaView.SelectSistemas = new SelectList(response, "Id", "Nome");
    }

    private async Task CarregarSelectProjetos()
    {
        IEnumerable<ProjetoViewModel> response = await _projetoGrpcService.AllAsync(new ListProjetoQuery { });
        TarefaView.SelectProjetos = new SelectList(response, "Id", "Nome");
    }

    private async Task CarregarSelectWorkflows()
    {
        IEnumerable<WorkflowViewModel> response = await _workflowGrpcService.AllAsync(new ListWorkflowQuery { });
        TarefaView.SelectWorkflows = new SelectList(response, "Id", "Nome");
    }

    private async Task CarregarSelectTipoTarefas()
    {
        IEnumerable<TipoTarefaViewModel> response = await _tipoTarefaGrpcService.AllAsync(new ListTipoTarefaQuery { });
        TarefaView.SelectTipoTarefas = new SelectList(response, "Id", "Nome");
    }
}
