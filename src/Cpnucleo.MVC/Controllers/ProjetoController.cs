using System;
using Cpnucleo.Domain.UoW;
using Microsoft.AspNetCore.Mvc;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.MVC.Controllers.V2
{
    [Authorize]
    public class ProjetoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjetoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private ProjetoViewModel _viewModel;

        public ProjetoViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = new ProjetoViewModel();

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
                ViewModel.Lista = _unitOfWork.ProjetoRepository.All(true);

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
            ViewModel.SelectSistemas = new SelectList(_unitOfWork.SistemaRepository.All(), "Id", "Nome");

            return View(ViewModel);
        }

        [HttpPost]
        public IActionResult Incluir(ProjetoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.SelectSistemas = new SelectList(_unitOfWork.SistemaRepository.All(), "Id", "Nome");

                    return View(ViewModel);
                }

                _unitOfWork.ProjetoRepository.Add(obj.Projeto);

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
                ViewModel.Projeto  = _unitOfWork.ProjetoRepository.Get(id);
                ViewModel.SelectSistemas = new SelectList(_unitOfWork.SistemaRepository.All(), "Id", "Nome");

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Alterar(ProjetoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Projeto  = _unitOfWork.ProjetoRepository.Get(obj.Projeto.Id);
                    ViewModel.SelectSistemas = new SelectList(_unitOfWork.SistemaRepository.All(), "Id", "Nome");

                    return View(ViewModel);
                }

                _unitOfWork.ProjetoRepository.Update(obj.Projeto);

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
                ViewModel.Projeto  = _unitOfWork.ProjetoRepository.Get(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Remover(ProjetoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Projeto  = _unitOfWork.ProjetoRepository.Get(obj.Projeto.Id);

                    return View(ViewModel);
                }

                _unitOfWork.ProjetoRepository.Remove(obj.Projeto.Id);

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
