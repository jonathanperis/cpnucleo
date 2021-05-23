using System;
using Cpnucleo.Domain.UoW;
using Microsoft.AspNetCore.Mvc;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
{
    [Authorize]
    public class WorkflowController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkflowController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private WorkflowViewModel _viewModel;

        public WorkflowViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = new WorkflowViewModel();

                return _viewModel;
            }
            set
            {
                _viewModel = value;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                ViewModel.Lista = await _unitOfWork.WorkflowRepository.AllAsync();

                return View(ViewModel);
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

                _unitOfWork.WorkflowRepository.AddAsync(obj.Workflow);

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
                ViewModel.Workflow  = await _unitOfWork.WorkflowRepository.GetAsync(id);

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
                    ViewModel.Workflow  = await _unitOfWork.WorkflowRepository.GetAsync(obj.Workflow.Id);

                    return View(ViewModel);
                }

                _unitOfWork.WorkflowRepository.Update(obj.Workflow);

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
                ViewModel.Workflow  = await _unitOfWork.WorkflowRepository.GetAsync(id);

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
                    ViewModel.Workflow  = await _unitOfWork.WorkflowRepository.GetAsync(obj.Workflow.Id);

                    return View(ViewModel);
                }

                await _unitOfWork.WorkflowRepository.RemoveAsync(obj.Workflow.Id);

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
