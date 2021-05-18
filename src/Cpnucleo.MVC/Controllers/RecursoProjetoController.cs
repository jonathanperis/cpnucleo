using System;
using Cpnucleo.Domain.UoW;
using Microsoft.AspNetCore.Mvc;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.MVC.Controllers.V2
{
    [Authorize]
    public class RecursoProjetoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecursoProjetoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private RecursoProjetoViewModel _viewModel;

        public RecursoProjetoViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = new RecursoProjetoViewModel();

                return _viewModel;
            }
            set
            {
                _viewModel = value;
            }
        }

        [HttpGet]
        public IActionResult Listar(Guid idProjeto)
        {
            try
            {
                ViewModel.Lista = _unitOfWork.RecursoProjetoRepository.GetByProjeto(idProjeto);

                ViewData["idProjeto"] = idProjeto;

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpGet]
        public IActionResult Incluir(Guid idProjeto)
        {
            ViewModel.Projeto = _unitOfWork.ProjetoRepository.Get(idProjeto);
            ViewModel.SelectRecursos = new SelectList(_unitOfWork.RecursoRepository.All(), "Id", "Nome");

            return View(ViewModel);
        }

        [HttpPost]
        public IActionResult Incluir(RecursoProjetoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Projeto = _unitOfWork.ProjetoRepository.Get(obj.Projeto.Id);
                    ViewModel.SelectRecursos = new SelectList(_unitOfWork.RecursoRepository.All(), "Id", "Nome");

                    return View(ViewModel);
                }

                _unitOfWork.RecursoProjetoRepository.Add(obj.RecursoProjeto);

                return RedirectToAction("Listar", new { idProjeto = obj.RecursoProjeto.IdProjeto });
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
                ViewModel.RecursoProjeto  = _unitOfWork.RecursoProjetoRepository.Get(id);
                ViewModel.SelectRecursos = new SelectList(_unitOfWork.RecursoRepository.All(), "Id", "Nome");

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Alterar(RecursoProjetoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.RecursoProjeto  = _unitOfWork.RecursoProjetoRepository.Get(obj.RecursoProjeto.Id);
                    ViewModel.SelectRecursos = new SelectList(_unitOfWork.RecursoRepository.All(), "Id", "Nome");

                    return View(ViewModel);
                }

                _unitOfWork.RecursoProjetoRepository.Update(obj.RecursoProjeto);

                return RedirectToAction("Listar", new { idProjeto = obj.RecursoProjeto.IdProjeto });
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
                ViewModel.RecursoProjeto  = _unitOfWork.RecursoProjetoRepository.Get(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Remover(RecursoProjetoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.RecursoProjeto  = _unitOfWork.RecursoProjetoRepository.Get(obj.RecursoProjeto.Id);

                    return View(ViewModel);
                }

                _unitOfWork.RecursoProjetoRepository.Remove(obj.RecursoProjeto.Id);

                return RedirectToAction("Listar", new { idProjeto = obj.RecursoProjeto.IdProjeto });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }
    }
}
