using System;
using Cpnucleo.Domain.UoW;
using Microsoft.AspNetCore.Mvc;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
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
        public async Task<IActionResult> Listar()
        {
            try
            {
                ViewModel.Lista = await _unitOfWork.ImpedimentoRepository.AllAsync();

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
        public async Task<IActionResult> Incluir(ImpedimentoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                await _unitOfWork.ImpedimentoRepository.AddAsync(obj.Impedimento);

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
                ViewModel.Impedimento  = await _unitOfWork.ImpedimentoRepository.GetAsync(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(ImpedimentoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Impedimento  = await _unitOfWork.ImpedimentoRepository.GetAsync(obj.Impedimento.Id);

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
        public async Task<IActionResult> Remover(Guid id)
        {
            try
            {
                ViewModel.Impedimento  = await _unitOfWork.ImpedimentoRepository.GetAsync(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remover(ImpedimentoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Impedimento  = await _unitOfWork.ImpedimentoRepository.GetAsync(obj.Impedimento.Id);

                    return View(ViewModel);
                }

                await _unitOfWork.ImpedimentoRepository.RemoveAsync(obj.Impedimento.Id);

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
