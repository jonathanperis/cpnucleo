using System;
using Cpnucleo.Domain.UoW;
using Microsoft.AspNetCore.Mvc;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.MVC.Controllers.V2
{
    [Authorize]
    public class ImpedimentoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImpedimentoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private ImpedimentoViewModel _viewModel;

        public ImpedimentoViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = new ImpedimentoViewModel();

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
                ViewModel.Lista = _unitOfWork.ImpedimentoRepository.All();

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
        public IActionResult Incluir(ImpedimentoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                _unitOfWork.ImpedimentoRepository.Add(obj.Impedimento);

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
                ViewModel.Impedimento  = _unitOfWork.ImpedimentoRepository.Get(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Alterar(ImpedimentoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Impedimento  = _unitOfWork.ImpedimentoRepository.Get(obj.Impedimento.Id);

                    return View(ViewModel);
                }

                _unitOfWork.ImpedimentoRepository.Update(obj.Impedimento);

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
                ViewModel.Impedimento  = _unitOfWork.ImpedimentoRepository.Get(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Remover(ImpedimentoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Impedimento  = _unitOfWork.ImpedimentoRepository.Get(obj.Impedimento.Id);

                    return View(ViewModel);
                }

                _unitOfWork.ImpedimentoRepository.Remove(obj.Impedimento.Id);

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
