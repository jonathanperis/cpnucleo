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
        public async Task<IActionResult> Listar(Guid idTarefa)
        {
            try
            {
                ViewModel.Lista = await _unitOfWork.ImpedimentoTarefaRepository.GetByTarefaAsync(idTarefa);

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
        public async Task<IActionResult> Incluir(Guid idTarefa)
        {
            ViewModel.Tarefa = await _unitOfWork.TarefaRepository.GetAsync(idTarefa);
            ViewModel.SelectImpedimentos = new SelectList(await _unitOfWork.ImpedimentoRepository.AllAsync(), "Id", "Nome");

            return View(ViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Incluir(ImpedimentoTarefaViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Tarefa = await _unitOfWork.TarefaRepository.GetAsync(obj.Tarefa.Id);
                    ViewModel.SelectImpedimentos = new SelectList(await _unitOfWork.ImpedimentoRepository.AllAsync(), "Id", "Nome");

                    return View(ViewModel);
                }

                await _unitOfWork.ImpedimentoTarefaRepository.AddAsync(obj.ImpedimentoTarefa);

                return RedirectToAction("Listar", new { idTarefa = obj.ImpedimentoTarefa.IdTarefa });
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
                ViewModel.ImpedimentoTarefa  = await _unitOfWork.ImpedimentoTarefaRepository.GetAsync(id);
                ViewModel.SelectImpedimentos = new SelectList(await _unitOfWork.ImpedimentoRepository.AllAsync(), "Id", "Nome");

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(ImpedimentoTarefaViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.ImpedimentoTarefa  = await _unitOfWork.ImpedimentoTarefaRepository.GetAsync(obj.ImpedimentoTarefa.Id);
                    ViewModel.SelectImpedimentos = new SelectList(await _unitOfWork.ImpedimentoRepository.AllAsync(), "Id", "Nome");

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
        public async Task<IActionResult> Remover(Guid id)
        {
            try
            {
                ViewModel.ImpedimentoTarefa  = await _unitOfWork.ImpedimentoTarefaRepository.GetAsync(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remover(ImpedimentoTarefaViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.ImpedimentoTarefa  = await _unitOfWork.ImpedimentoTarefaRepository.GetAsync(obj.ImpedimentoTarefa.Id);

                    return View(ViewModel);
                }

                await _unitOfWork.ImpedimentoTarefaRepository.RemoveAsync(obj.ImpedimentoTarefa.Id);

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
