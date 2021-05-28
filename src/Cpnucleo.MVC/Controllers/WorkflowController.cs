using Cpnucleo.Application.Interfaces;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
{
    [Authorize]
    public class WorkflowController : Controller
    {
        private readonly IWorkflowAppService _workflowAppService;

        private WorkflowView _workflowView;

        public WorkflowController(IWorkflowAppService workflowAppService)
        {
            _workflowAppService = workflowAppService;
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
                WorkflowView.Lista = await _workflowAppService.AllAsync();

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

                await _workflowAppService.AddAsync(obj.Workflow);
                await _workflowAppService.SaveChangesAsync();

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
                WorkflowView.Workflow = await _workflowAppService.GetAsync(id);

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
                    WorkflowView.Workflow = await _workflowAppService.GetAsync(obj.Workflow.Id);

                    return View(WorkflowView);
                }

                _workflowAppService.Update(obj.Workflow);
                await _workflowAppService.SaveChangesAsync();

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
                WorkflowView.Workflow = await _workflowAppService.GetAsync(id);

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
                    WorkflowView.Workflow = await _workflowAppService.GetAsync(obj.Workflow.Id);

                    return View(WorkflowView);
                }

                await _workflowAppService.RemoveAsync(obj.Workflow.Id);
                await _workflowAppService.SaveChangesAsync();

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
