using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.CreateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.RemoveWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.UpdateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.GetWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.ListWorkflow;

namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class WorkflowController : BaseController
{
    private readonly IWorkflowGrpcService _workflowGrpcService;

    private WorkflowView _workflowView;

    public WorkflowController(IConfiguration configuration)
        : base(configuration)
    {
        _workflowGrpcService = MagicOnionClient.Create<IWorkflowGrpcService>(CreateAuthenticatedChannel());
    }

    public WorkflowView WorkflowView
    {
        get
        {
            if (_workflowView == null)
            {
                _workflowView = new WorkflowView();
            }

            return _workflowView;
        }
        set => _workflowView = value;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        try
        {
            ListWorkflowResponse response = await _workflowGrpcService.AllAsync(new ListWorkflowQuery { });
            WorkflowView.Lista = response.Workflows;

            return View(WorkflowView);
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
    public async Task<IActionResult> Incluir(WorkflowView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _workflowGrpcService.AddAsync(new CreateWorkflowCommand { Workflow = obj.Workflow });

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
            GetWorkflowResponse response = await _workflowGrpcService.GetAsync(new GetWorkflowQuery { Id = id });
            WorkflowView.Workflow = response.Workflow;

            return View(WorkflowView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Alterar(WorkflowView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                GetWorkflowResponse response = await _workflowGrpcService.GetAsync(new GetWorkflowQuery { Id = obj.Workflow.Id });
                WorkflowView.Workflow = response.Workflow;

                return View(WorkflowView);
            }

            await _workflowGrpcService.UpdateAsync(new UpdateWorkflowCommand { Workflow = obj.Workflow });

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
            GetWorkflowResponse response = await _workflowGrpcService.GetAsync(new GetWorkflowQuery { Id = id });
            WorkflowView.Workflow = response.Workflow;

            return View(WorkflowView);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Remover(WorkflowView obj)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                GetWorkflowResponse response = await _workflowGrpcService.GetAsync(new GetWorkflowQuery { Id = obj.Workflow.Id });
                WorkflowView.Workflow = response.Workflow;

                return View(WorkflowView);
            }

            await _workflowGrpcService.RemoveAsync(new RemoveWorkflowCommand { Id = obj.Workflow.Id });

            return RedirectToAction("Listar");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }
}
