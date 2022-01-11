using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow;

namespace Cpnucleo.MVC.Controllers;

[Authorize]
public class WorkflowController : BaseController
{
    private readonly IWorkflowGrpcService _workflowGrpcService;

    private WorkflowView _workflowView;

    public WorkflowController(IConfiguration configuration)
    {
        _workflowGrpcService = MagicOnionClient.Create<IWorkflowGrpcService>(CreateAuthenticatedChannel(configuration["AppSettings:UrlCpnucleoGrpcWorkflow"]));
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
            WorkflowView.Lista = await _workflowGrpcService.AllAsync(new ListWorkflowQuery { });

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
            WorkflowView.Workflow = await _workflowGrpcService.GetAsync(new GetWorkflowQuery { Id = id });

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
                WorkflowView.Workflow = await _workflowGrpcService.GetAsync(new GetWorkflowQuery { Id = obj.Workflow.Id });

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
            WorkflowView.Workflow = await _workflowGrpcService.GetAsync(new GetWorkflowQuery { Id = id });

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
                WorkflowView.Workflow = await _workflowGrpcService.GetAsync(new GetWorkflowQuery { Id = obj.Workflow.Id });

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
