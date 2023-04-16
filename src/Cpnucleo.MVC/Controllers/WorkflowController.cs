using Cpnucleo.Shared.Commands.CreateWorkflow;
using Cpnucleo.Shared.Commands.RemoveWorkflow;
using Cpnucleo.Shared.Commands.UpdateWorkflow;
using Cpnucleo.Shared.Queries.GetWorkflow;
using Cpnucleo.Shared.Queries.ListWorkflow;

namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class WorkflowController : BaseController
{
    private readonly IWorkflowGrpcService _workflowGrpcService;

    private WorkflowViewModel _viewModel;

    public WorkflowController(IConfiguration configuration)
        : base(configuration)
    {
        _workflowGrpcService = MagicOnionClient.Create<IWorkflowGrpcService>(CreateAuthenticatedChannel());
    }

    public WorkflowViewModel ViewModel
    {
        get
        {
            if (_viewModel == null)
            {
                _viewModel = new WorkflowViewModel();
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
            var result = await _workflowGrpcService.ListWorkflow(new ListWorkflowQuery());

            if (result.OperationResult == OperationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
                return View();
            }

            ViewModel.Lista = result.Workflows;

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
    public async Task<IActionResult> Incluir(WorkflowViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _workflowGrpcService.CreateWorkflow(new CreateWorkflowCommand(obj.Workflow.Nome, obj.Workflow.Ordem));

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
    public async Task<IActionResult> Alterar(WorkflowViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(obj.Workflow.Id);

                return View(ViewModel);
            }

            var result = await _workflowGrpcService.UpdateWorkflow(new UpdateWorkflowCommand(obj.Workflow.Id, obj.Workflow.Nome, obj.Workflow.Ordem));

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
    public async Task<IActionResult> Remover(WorkflowViewModel obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                await CarregarDados(obj.Workflow.Id);

                return View(ViewModel);
            }

            var result = await _workflowGrpcService.RemoveWorkflow(new RemoveWorkflowCommand(obj.Workflow.Id));

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
        var result = await _workflowGrpcService.GetWorkflow(new GetWorkflowQuery(id));

        if (result.OperationResult == OperationResult.Failed)
        {
            ModelState.AddModelError(string.Empty, "Não foi possível processar a solicitação no momento.");
            return;
        }

        ViewModel.Workflow = result.Workflow;
    }
}
