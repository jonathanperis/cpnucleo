using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.CreateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.RemoveTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.UpdateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.ListProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.ListSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.ListTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.ListTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.ListWorkflow;

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
            ListTarefaResponse response = await _tarefaGrpcService.AllAsync(new ListTarefaQuery { GetDependencies = true });
            TarefaView.Lista = response.Tarefas;

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
            GetTarefaResponse response = await _tarefaGrpcService.GetAsync(new GetTarefaQuery { Id = id });
            TarefaView.Tarefa = response.Tarefa;

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
                GetTarefaResponse response = await _tarefaGrpcService.GetAsync(new GetTarefaQuery { Id = obj.Tarefa.Id });
                TarefaView.Tarefa = response.Tarefa;

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
            GetTarefaResponse response = await _tarefaGrpcService.GetAsync(new GetTarefaQuery { Id = id });
            TarefaView.Tarefa = response.Tarefa;

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
                GetTarefaResponse response = await _tarefaGrpcService.GetAsync(new GetTarefaQuery { Id = obj.Tarefa.Id });
                TarefaView.Tarefa = response.Tarefa;

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
        ListSistemaResponse response = await _sistemaGrpcService.AllAsync(new ListSistemaQuery { });
        TarefaView.SelectSistemas = new SelectList(response.Sistemas, "Id", "Nome");
    }

    private async Task CarregarSelectProjetos()
    {
        ListProjetoResponse response = await _projetoGrpcService.AllAsync(new ListProjetoQuery { });
        TarefaView.SelectProjetos = new SelectList(response.Projetos, "Id", "Nome");
    }

    private async Task CarregarSelectWorkflows()
    {
        ListWorkflowResponse response = await _workflowGrpcService.AllAsync(new ListWorkflowQuery { });
        TarefaView.SelectWorkflows = new SelectList(response.Workflows, "Id", "Nome");
    }

    private async Task CarregarSelectTipoTarefas()
    {
        ListTipoTarefaResponse response = await _tipoTarefaGrpcService.AllAsync(new ListTipoTarefaQuery { });
        TarefaView.SelectTipoTarefas = new SelectList(response.TipoTarefas, "Id", "Nome");
    }
}
