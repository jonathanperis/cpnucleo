using System;
using Cpnucleo.Domain.UoW;
using Microsoft.AspNetCore.Mvc;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
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
        public async Task<IActionResult> Listar()
        {
            try
            {
                ViewModel.Lista = await _unitOfWork.ProjetoRepository.AllAsync(true);

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
            ViewModel.SelectSistemas = new SelectList(await _unitOfWork.SistemaRepository.AllAsync(), "Id", "Nome");

            return View(ViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Incluir(ProjetoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.SelectSistemas = new SelectList(await _unitOfWork.SistemaRepository.AllAsync(), "Id", "Nome");

                    return View(ViewModel);
                }

                await _unitOfWork.ProjetoRepository.AddAsync(obj.Projeto);

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
                ViewModel.Projeto  = await _unitOfWork.ProjetoRepository.GetAsync(id);
                ViewModel.SelectSistemas = new SelectList(await _unitOfWork.SistemaRepository.AllAsync(), "Id", "Nome");

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(ProjetoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Projeto  = await _unitOfWork.ProjetoRepository.GetAsync(obj.Projeto.Id);
                    ViewModel.SelectSistemas = new SelectList(await _unitOfWork.SistemaRepository.AllAsync(), "Id", "Nome");

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
        public async Task<IActionResult> Remover(Guid id)
        {
            try
            {
                ViewModel.Projeto  = await _unitOfWork.ProjetoRepository.GetAsync(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remover(ProjetoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Projeto  = await _unitOfWork.ProjetoRepository.GetAsync(obj.Projeto.Id);

                    return View(ViewModel);
                }

                await _unitOfWork.ProjetoRepository.RemoveAsync(obj.Projeto.Id);

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
