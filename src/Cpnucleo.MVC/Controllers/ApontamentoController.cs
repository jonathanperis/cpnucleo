using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow;
using Cpnucleo.MVC.Services;
using System.Security.Claims;

namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class ApontamentoController : BaseController
{
    private readonly IApontamentoGrpcService _apontamentoGrpcService;
    private readonly ITarefaGrpcService _tarefaGrpcService;
    private readonly IWorkflowGrpcService _workflowGrpcService;

    private ApontamentoView _apontamentoView;

    public ApontamentoController(IConfiguration configuration)
    {
        _apontamentoGrpcService = MagicOnionClient.Create<IApontamentoGrpcService>(CreateAuthenticatedChannel(configuration["AppSettings:UrlCpnucleoGrpcApontamento"]));
        _tarefaGrpcService = MagicOnionClient.Create<ITarefaGrpcService>(CreateAuthenticatedChannel(configuration["AppSettings:UrlCpnucleoGrpcTarefa"]));
        _workflowGrpcService = MagicOnionClient.Create<IWorkflowGrpcService>(CreateAuthenticatedChannel(configuration["AppSettings:UrlCpnucleoGrpcWorkflow"]));
    }

    public ApontamentoView ApontamentoView
    {
        get
        {
            if (_apontamentoView == null)
            {
                _apontamentoView = new ApontamentoView();
            }

            return _apontamentoView;
        }
        set => _apontamentoView = value;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        try
        {
            string retorno = ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
            Guid idRecurso = new(retorno);

            ApontamentoView.Lista = await _apontamentoGrpcService.GetByRecursoAsync(new Infra.CrossCutting.Util.Queries.Apontamento.GetByRecursoQuery { IdRecurso = idRecurso });

            await CarregarTarefasByRecurso(idRecurso);

            return View(ApontamentoView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Listar(ApontamentoView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                string retorno = ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
                Guid idRecurso = new(retorno);

                ApontamentoView.Lista = await _apontamentoGrpcService.GetByRecursoAsync(new Infra.CrossCutting.Util.Queries.Apontamento.GetByRecursoQuery { IdRecurso = idRecurso });

                await CarregarTarefasByRecurso(idRecurso);

                return View(ApontamentoView);
            }

            await _apontamentoGrpcService.AddAsync(new CreateApontamentoCommand { Apontamento = obj.Apontamento });

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
            ApontamentoView.Apontamento = await _apontamentoGrpcService.GetAsync(new GetApontamentoQuery { Id = id });

            return View(ApontamentoView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Remover(ApontamentoView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ApontamentoView.Apontamento = await _apontamentoGrpcService.GetAsync(new GetApontamentoQuery { Id = obj.Apontamento.Id });

                return View(ApontamentoView);
            }

            await _apontamentoGrpcService.RemoveAsync(new RemoveApontamentoCommand { Id = obj.Apontamento.Id });

            return RedirectToAction("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> FluxoTrabalho()
    {
        try
        {
            await CarregarWorkflows();
            await CarregarTarefas();

            return View(ApontamentoView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<JsonResult> FluxoTrabalho(Guid idTarefa, Guid idWorkflow)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarWorkflows();
                await CarregarTarefas();

                return Json(new { success = false, message = "", body = ApontamentoView });
            }

            TarefaViewModel tarefa = await _tarefaGrpcService.GetAsync(new GetTarefaQuery { Id = idTarefa });
            WorkflowViewModel workflow = await _workflowGrpcService.GetAsync(new GetWorkflowQuery { Id = idWorkflow });

            tarefa.IdWorkflow = workflow.Id;
            tarefa.Workflow = workflow;

            await _tarefaGrpcService.UpdateAsync(new UpdateTarefaCommand { Tarefa = tarefa });

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Json(new { success = false, message = ex.Message });
        }
    }

    private async Task CarregarTarefasByRecurso(Guid idRecurso)
    {
        ApontamentoView.ListaTarefas = await _tarefaGrpcService.GetByRecursoAsync(new Infra.CrossCutting.Util.Queries.Tarefa.GetByRecursoQuery { IdRecurso = idRecurso });
    }

    private async Task CarregarWorkflows()
    {
        ApontamentoView.ListaWorkflow = await _workflowGrpcService.AllAsync(new ListWorkflowQuery { });
    }

    private async Task CarregarTarefas()
    {
        ApontamentoView.ListaTarefas = await _tarefaGrpcService.AllAsync(new ListTarefaQuery { GetDependencies = true });
    }
}
