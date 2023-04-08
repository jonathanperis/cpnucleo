using Cpnucleo.MVC.Services;
using Cpnucleo.Shared.Commands.CreateApontamento;
using Cpnucleo.Shared.Commands.RemoveApontamento;
using Cpnucleo.Shared.Commands.UpdateTarefaByWorkflow;
using Cpnucleo.Shared.Queries.GetApontamento;
using Cpnucleo.Shared.Queries.ListApontamentoByRecurso;
using Cpnucleo.Shared.Queries.ListTarefa;
using Cpnucleo.Shared.Queries.ListTarefaByRecurso;
using Cpnucleo.Shared.Queries.ListWorkflow;
using System.Security.Claims;

namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class ApontamentoController : BaseController
{
    private readonly IApontamentoGrpcService _apontamentoGrpcService;
    private readonly ITarefaGrpcService _tarefaGrpcService;
    private readonly IWorkflowGrpcService _workflowGrpcService;

    private ApontamentoViewModel _viewModel;

    public ApontamentoController(IConfiguration configuration)
        : base(configuration)
    {
        _apontamentoGrpcService = MagicOnionClient.Create<IApontamentoGrpcService>(CreateAuthenticatedChannel());
        _tarefaGrpcService = MagicOnionClient.Create<ITarefaGrpcService>(CreateAuthenticatedChannel());
        _workflowGrpcService = MagicOnionClient.Create<IWorkflowGrpcService>(CreateAuthenticatedChannel());
    }

    public ApontamentoViewModel ViewModel
    {
        get
        {
            if (_viewModel == null)
            {
                _viewModel = new ApontamentoViewModel();
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
            await CarregarDadosListar();

            return View(ViewModel);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Listar(ApontamentoViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDadosListar();

                return View(ViewModel);
            }

            OperationResult result = await _apontamentoGrpcService.CreateApontamento(new CreateApontamentoCommand(obj.Apontamento.Descricao, obj.Apontamento.DataApontamento, obj.Apontamento.QtdHoras, obj.Apontamento.IdTarefa, obj.Apontamento.IdRecurso));

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
    public async Task<IActionResult> Remover(ApontamentoViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(obj.Apontamento.Id);

                return View(ViewModel);
            }

            OperationResult result = await _apontamentoGrpcService.RemoveApontamento(new RemoveApontamentoCommand(obj.Apontamento.Id));

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
    public async Task<IActionResult> FluxoTrabalho()
    {
        try
        {
            await CarregarDadosFluxoTrabalho();

            return View(ViewModel);
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
                await CarregarDadosFluxoTrabalho();

                return Json(new { success = false, message = "", body = ViewModel });
            }

            OperationResult result = await _tarefaGrpcService.UpdateTarefaByWorkflow(new UpdateTarefaByWorkflowCommand(idTarefa, idWorkflow));

            if (result == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return Json(new { success = false, message = "", body = ViewModel });
            }

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Json(new { success = false, message = ex.Message });
        }
    }

    private async Task CarregarDados(Guid id)
    {
        GetApontamentoViewModel result = await _apontamentoGrpcService.GetApontamento(new GetApontamentoQuery(id));

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        ViewModel.Apontamento = result.Apontamento;
    }

    private async Task CarregarDadosListar()
    {
        string retorno = ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
        Guid idRecurso = new(retorno);

        ListApontamentoByRecursoViewModel result = await _apontamentoGrpcService.GetApontamentoByRecurso(new ListApontamentoByRecursoQuery(idRecurso));

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        ViewModel.Lista = result.Apontamentos;

        ListTarefaByRecursoViewModel result2 = await _tarefaGrpcService.GetTarefaByRecurso(new ListTarefaByRecursoQuery(idRecurso));

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        ViewModel.ListaTarefas = result2.Tarefas;
    }

    private async Task CarregarDadosFluxoTrabalho()
    {
        ListWorkflowViewModel result = await _workflowGrpcService.ListWorkflow(new ListWorkflowQuery());

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        ViewModel.ListaWorkflow = result.Workflows;

        ListTarefaViewModel result2 = await _tarefaGrpcService.ListTarefa(new ListTarefaQuery { GetDependencies = true });

        if (result2.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        ViewModel.ListaTarefas = result2.Tarefas;
    }
}
