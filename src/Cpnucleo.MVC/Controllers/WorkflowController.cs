using System;
using Cpnucleo.Domain.UoW;
using Microsoft.AspNetCore.Mvc;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.MVC.Controllers.V2
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
        public IActionResult Listar()
        {
            try
            {
                ViewModel.Lista = _unitOfWork.WorkflowRepository.All();

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
        public IActionResult Incluir(WorkflowViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                _unitOfWork.WorkflowRepository.Add(obj.Workflow);

                return RedirectToAction("Listar");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpGet]
        public IActionResult Alterar(Guid id)
        {
            try
            {
                ViewModel.Workflow  = _unitOfWork.WorkflowRepository.Get(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Alterar(WorkflowViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Workflow  = _unitOfWork.WorkflowRepository.Get(obj.Workflow.Id);

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
        public IActionResult Remover(Guid id)
        {
            try
            {
                ViewModel.Workflow  = _unitOfWork.WorkflowRepository.Get(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Remover(WorkflowViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Workflow  = _unitOfWork.WorkflowRepository.Get(obj.Workflow.Id);

                    return View(ViewModel);
                }

                _unitOfWork.WorkflowRepository.Remove(obj.Workflow.Id);

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
