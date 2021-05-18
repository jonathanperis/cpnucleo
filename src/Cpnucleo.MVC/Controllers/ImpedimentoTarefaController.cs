using System;
using Cpnucleo.Domain.UoW;
using Microsoft.AspNetCore.Mvc;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.MVC.Controllers.V2
{
    [Authorize]
    public class ImpedimentoTarefaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImpedimentoTarefaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private ImpedimentoTarefaViewModel _viewModel;

        public ImpedimentoTarefaViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = new ImpedimentoTarefaViewModel();

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
                ViewModel.Lista = _unitOfWork.ImpedimentoTarefaRepository.GetByTarefa(idTarefa);

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
            ViewModel.SelectImpedimentos = new SelectList(_unitOfWork.ImpedimentoRepository.All(), "Id", "Nome");

            return View(ViewModel);
        }

        [HttpPost]
        public IActionResult Incluir(ImpedimentoTarefaViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Tarefa = _unitOfWork.TarefaRepository.Get(obj.Tarefa.Id);
                    ViewModel.SelectImpedimentos = new SelectList(_unitOfWork.ImpedimentoRepository.All(), "Id", "Nome");

                    return View(ViewModel);
                }

                _unitOfWork.ImpedimentoTarefaRepository.Add(obj.ImpedimentoTarefa);

                return RedirectToAction("Listar", new { idTarefa = obj.ImpedimentoTarefa.IdTarefa });
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
                ViewModel.ImpedimentoTarefa  = _unitOfWork.ImpedimentoTarefaRepository.Get(id);
                ViewModel.SelectImpedimentos = new SelectList(_unitOfWork.ImpedimentoRepository.All(), "Id", "Nome");

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Alterar(ImpedimentoTarefaViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.ImpedimentoTarefa  = _unitOfWork.ImpedimentoTarefaRepository.Get(obj.ImpedimentoTarefa.Id);
                    ViewModel.SelectImpedimentos = new SelectList(_unitOfWork.ImpedimentoRepository.All(), "Id", "Nome");

                    return View(ViewModel);
                }

                _unitOfWork.ImpedimentoTarefaRepository.Update(obj.ImpedimentoTarefa);

                return RedirectToAction("Listar", new { idTarefa = obj.ImpedimentoTarefa.IdTarefa });
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
                ViewModel.ImpedimentoTarefa  = _unitOfWork.ImpedimentoTarefaRepository.Get(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Remover(ImpedimentoTarefaViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.ImpedimentoTarefa  = _unitOfWork.ImpedimentoTarefaRepository.Get(obj.ImpedimentoTarefa.Id);

                    return View(ViewModel);
                }

                _unitOfWork.ImpedimentoTarefaRepository.Remove(obj.ImpedimentoTarefa.Id);

                return RedirectToAction("Listar", new { idTarefa = obj.ImpedimentoTarefa.IdTarefa });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }
    }
}
