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
        public async Task<IActionResult> Listar(Guid idTarefa)
        {
            try
            {
                ViewModel.Lista = await _unitOfWork.RecursoTarefaRepository.GetByTarefaAsync(idTarefa);

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
            ViewModel.SelectRecursos = new SelectList(await _unitOfWork.RecursoRepository.AllAsync(), "Id", "Nome");

            return View(ViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Incluir(RecursoTarefaViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Tarefa = await _unitOfWork.TarefaRepository.GetAsync(obj.Tarefa.Id);
                    ViewModel.SelectRecursos = new SelectList(await _unitOfWork.RecursoRepository.AllAsync(), "Id", "Nome");

                    return View(ViewModel);
                }

                await _unitOfWork.RecursoTarefaRepository.AddAsync(obj.RecursoTarefa);

                return RedirectToAction("Listar", new { idTarefa = obj.RecursoTarefa.IdTarefa });
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
                ViewModel.RecursoTarefa  = await _unitOfWork.RecursoTarefaRepository.GetAsync(id);
                ViewModel.SelectRecursos = new SelectList(await _unitOfWork.RecursoRepository.AllAsync(), "Id", "Nome");

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(RecursoTarefaViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.RecursoTarefa  = await _unitOfWork.RecursoTarefaRepository.GetAsync(obj.RecursoTarefa.Id);
                    ViewModel.SelectRecursos = new SelectList(await _unitOfWork.RecursoRepository.AllAsync(), "Id", "Nome");

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
        public async Task<IActionResult> Remover(Guid id)
        {
            try
            {
                ViewModel.RecursoTarefa  = await _unitOfWork.RecursoTarefaRepository.GetAsync(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remover(RecursoTarefaViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.RecursoTarefa  = await _unitOfWork.RecursoTarefaRepository.GetAsync(obj.RecursoTarefa.Id);

                    return View(ViewModel);
                }

                await _unitOfWork.RecursoTarefaRepository.RemoveAsync(obj.RecursoTarefa.Id);

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
