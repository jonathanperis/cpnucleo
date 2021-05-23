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

        private ImpedimentoView _viewModel;

        public ImpedimentoView ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = new ImpedimentoView();

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
        public async Task<IActionResult> Incluir(ImpedimentoView obj)
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
        public async Task<IActionResult> Alterar(ImpedimentoView obj)
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
        public async Task<IActionResult> Remover(ImpedimentoView obj)
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
