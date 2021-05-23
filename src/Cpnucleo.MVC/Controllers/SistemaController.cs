using System;
using Cpnucleo.Domain.UoW;
using Microsoft.AspNetCore.Mvc;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
{
    [Authorize]
    public class SistemaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SistemaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private SistemaView _viewModel;

        public SistemaView ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = new SistemaView();

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
                ViewModel.Lista = await _unitOfWork.SistemaRepository.AllAsync();

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
        public async Task<IActionResult> Incluir(SistemaView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                await _unitOfWork.SistemaRepository.AddAsync(obj.Sistema);

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
                ViewModel.Sistema  = await _unitOfWork.SistemaRepository.GetAsync(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(SistemaView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Sistema  = await _unitOfWork.SistemaRepository.GetAsync(obj.Sistema.Id);

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
        public async Task<IActionResult> Remover(Guid id)
        {
            try
            {
                ViewModel.Sistema  = await _unitOfWork.SistemaRepository.GetAsync(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remover(SistemaView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Sistema  = await _unitOfWork.SistemaRepository.GetAsync(obj.Sistema.Id);

                    return View(ViewModel);
                }

                await _unitOfWork.SistemaRepository.RemoveAsync(obj.Sistema.Id);

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
