using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.CreateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.RemoveWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.UpdateWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.GetWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.ListWorkflow;
using Cpnucleo.MVC.Interfaces;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
{
    [Authorize]
    public class WorkflowController : BaseController
    {
        private readonly IWorkflowService _workflowService;

        private WorkflowView _workflowView;

        public WorkflowController(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
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
                ListWorkflowResponse response = await _workflowService.AllAsync(Token, new ListWorkflowQuery { });
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

                await _workflowService.AddAsync(Token, new CreateWorkflowCommand { Workflow = obj.Workflow });

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
                GetWorkflowResponse response = await _workflowService.GetAsync(Token, new GetWorkflowQuery { Id = id });
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
                    GetWorkflowResponse response = await _workflowService.GetAsync(Token, new GetWorkflowQuery { Id = obj.Workflow.Id });
                    WorkflowView.Workflow = response.Workflow;

                    return View(WorkflowView);
                }

                await _workflowService.UpdateAsync(Token, new UpdateWorkflowCommand { Workflow = obj.Workflow });

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
                GetWorkflowResponse response = await _workflowService.GetAsync(Token, new GetWorkflowQuery { Id = id });
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
                    GetWorkflowResponse response = await _workflowService.GetAsync(Token, new GetWorkflowQuery { Id = obj.Workflow.Id });
                    WorkflowView.Workflow = response.Workflow;

                    return View(WorkflowView);
                }

                await _workflowService.RemoveAsync(Token, new RemoveWorkflowCommand { Id = obj.Workflow.Id });

                return RedirectToAction("Listar");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }
    }
}
