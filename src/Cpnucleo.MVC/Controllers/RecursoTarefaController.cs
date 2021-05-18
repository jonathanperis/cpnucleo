using System;
using Cpnucleo.Domain.UoW;
using Microsoft.AspNetCore.Mvc;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.MVC.Controllers.V2
{
    [Authorize]
    public class RecursoTarefaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RecursoTarefaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private RecursoTarefaViewModel _viewModel;

        public RecursoTarefaViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = new RecursoTarefaViewModel();

                return _viewModel;
            }
            set
            {
                _viewModel = value;
            }
        }

        [HttpGet]
        public IActionResult Listar(Guid idTarefa)
        {
            try
            {
                ViewModel.Lista = _unitOfWork.RecursoTarefaRepository.GetByTarefa(idTarefa);

                ViewData["idTarefa"] = idTarefa;

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpGet]
        public IActionResult Incluir(Guid idTarefa)
        {
            ViewModel.Tarefa = _unitOfWork.TarefaRepository.Get(idTarefa);
            ViewModel.SelectRecursos = new SelectList(_unitOfWork.RecursoRepository.All(), "Id", "Nome");

            return View(ViewModel);
        }

        [HttpPost]
        public IActionResult Incluir(RecursoTarefaViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Tarefa = _unitOfWork.TarefaRepository.Get(obj.Tarefa.Id);
                    ViewModel.SelectRecursos = new SelectList(_unitOfWork.RecursoRepository.All(), "Id", "Nome");

                    return View(ViewModel);
                }

                _unitOfWork.RecursoTarefaRepository.Add(obj.RecursoTarefa);

                return RedirectToAction("Listar", new { idTarefa = obj.RecursoTarefa.IdTarefa });
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
                ViewModel.RecursoTarefa  = _unitOfWork.RecursoTarefaRepository.Get(id);
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
        public IActionResult Alterar(RecursoTarefaViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.RecursoTarefa  = _unitOfWork.RecursoTarefaRepository.Get(obj.RecursoTarefa.Id);
                    ViewModel.SelectRecursos = new SelectList(_unitOfWork.RecursoRepository.All(), "Id", "Nome");

                    return View(ViewModel);
                }

                _unitOfWork.RecursoTarefaRepository.Update(obj.RecursoTarefa);

                return RedirectToAction("Listar", new { idTarefa = obj.RecursoTarefa.IdTarefa });
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
                ViewModel.RecursoTarefa  = _unitOfWork.RecursoTarefaRepository.Get(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Remover(RecursoTarefaViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.RecursoTarefa  = _unitOfWork.RecursoTarefaRepository.Get(obj.RecursoTarefa.Id);

                    return View(ViewModel);
                }

                _unitOfWork.RecursoTarefaRepository.Remove(obj.RecursoTarefa.Id);

                return RedirectToAction("Listar", new { idTarefa = obj.RecursoTarefa.IdTarefa });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }
    }
}
