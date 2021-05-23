using System;
using Cpnucleo.Domain.UoW;
using Microsoft.AspNetCore.Mvc;
using Cpnucleo.MVC.Models;
using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
{
    [Authorize]
    public class RecursoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICryptographyManager _cryptographyManager;

        public RecursoController(IUnitOfWork unitOfWork, ICryptographyManager cryptographyManager)
        {
            _unitOfWork = unitOfWork;
            _cryptographyManager = cryptographyManager;
        }

        private RecursoViewModel _viewModel;

        public RecursoViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = new RecursoViewModel();

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
                ViewModel.Lista = await _unitOfWork.RecursoRepository.AllAsync();

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
        public async Task<IActionResult> Incluir(RecursoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                _cryptographyManager.CryptPbkdf2(obj.Recurso.Senha, out string senhaCrypt, out string salt);

                obj.Recurso.Senha = senhaCrypt;
                obj.Recurso.Salt = salt;

                await _unitOfWork.RecursoRepository.AddAsync(obj.Recurso);

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
                ViewModel.Recurso  = await _unitOfWork.RecursoRepository.GetAsync(id);

                ViewModel.Recurso.Senha = null;
                ViewModel.Recurso.Salt = null;

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(RecursoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Recurso  = await _unitOfWork.RecursoRepository.GetAsync(obj.Recurso.Id);

                    ViewModel.Recurso.Senha = null;
                    ViewModel.Recurso.Salt = null;                    

                    return View(ViewModel);
                }

                _cryptographyManager.CryptPbkdf2(obj.Recurso.Senha, out string senhaCrypt, out string salt);

                obj.Recurso.Senha = senhaCrypt;
                obj.Recurso.Salt = salt;                

                _unitOfWork.RecursoRepository.Update(obj.Recurso);

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
                ViewModel.Recurso  = await _unitOfWork.RecursoRepository.GetAsync(id);

                ViewModel.Recurso.Senha = null;
                ViewModel.Recurso.Salt = null;                

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remover(RecursoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Recurso  = await _unitOfWork.RecursoRepository.GetAsync(obj.Recurso.Id);

                    ViewModel.Recurso.Senha = null;
                    ViewModel.Recurso.Salt = null;                    

                    return View(ViewModel);
                }

                await _unitOfWork.RecursoRepository.RemoveAsync(obj.Recurso.Id);

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
