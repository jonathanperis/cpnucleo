using System;
using Cpnucleo.Domain.UoW;
using Microsoft.AspNetCore.Mvc;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.MVC.Controllers.V2
{
    [Authorize]
    public class SistemaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SistemaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private SistemaViewModel _viewModel;

        public SistemaViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = new SistemaViewModel();

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
                ViewModel.Lista = _unitOfWork.SistemaRepository.All();

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
        public IActionResult Incluir(SistemaViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                _unitOfWork.SistemaRepository.Add(obj.Sistema);

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
                ViewModel.Sistema  = _unitOfWork.SistemaRepository.Get(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Alterar(SistemaViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Sistema  = _unitOfWork.SistemaRepository.Get(obj.Sistema.Id);

                    return View(ViewModel);
                }

                _unitOfWork.SistemaRepository.Update(obj.Sistema);

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
                ViewModel.Sistema  = _unitOfWork.SistemaRepository.Get(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Remover(SistemaViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Sistema  = _unitOfWork.SistemaRepository.Get(obj.Sistema.Id);

                    return View(ViewModel);
                }

                _unitOfWork.SistemaRepository.Remove(obj.Sistema.Id);

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
