using System;
using Cpnucleo.Domain.UoW;
using Microsoft.AspNetCore.Mvc;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace Cpnucleo.MVC.Controllers.V2
{
    [Authorize]
    public class TarefaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TarefaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private TarefaViewModel _viewModel;

        public TarefaViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = new TarefaViewModel();

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
                ViewModel.Lista = _unitOfWork.TarefaRepository.All(true);

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
            ViewModel.SelectProjetos = new SelectList(_unitOfWork.ProjetoRepository.All(), "Id", "Nome");
            ViewModel.SelectSistemas = new SelectList(_unitOfWork.SistemaRepository.All(), "Id", "Nome");
            ViewModel.SelectWorkflows = new SelectList(_unitOfWork.WorkflowRepository.All(), "Id", "Nome");
            ViewModel.SelectTipoTarefas = new SelectList(_unitOfWork.TipoTarefaRepository.All(), "Id", "Nome");

            ViewModel.User = HttpContext.User;

            return View(ViewModel);
        }

        [HttpPost]
        public IActionResult Incluir(TarefaViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.SelectProjetos = new SelectList(_unitOfWork.ProjetoRepository.All(), "Id", "Nome");
                    ViewModel.SelectSistemas = new SelectList(_unitOfWork.SistemaRepository.All(), "Id", "Nome");
                    ViewModel.SelectWorkflows = new SelectList(_unitOfWork.WorkflowRepository.All(), "Id", "Nome");
                    ViewModel.SelectTipoTarefas = new SelectList(_unitOfWork.TipoTarefaRepository.All(), "Id", "Nome");
                    
                    ViewModel.User = HttpContext.User;  

                    return View(ViewModel);
                }

                _unitOfWork.TarefaRepository.Add(obj.Tarefa);

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
                ViewModel.Tarefa  = _unitOfWork.TarefaRepository.Get(id);
                
                ViewModel.SelectProjetos = new SelectList(_unitOfWork.ProjetoRepository.All(), "Id", "Nome");
                ViewModel.SelectSistemas = new SelectList(_unitOfWork.SistemaRepository.All(), "Id", "Nome");
                ViewModel.SelectWorkflows = new SelectList(_unitOfWork.WorkflowRepository.All(), "Id", "Nome");
                ViewModel.SelectTipoTarefas = new SelectList(_unitOfWork.TipoTarefaRepository.All(), "Id", "Nome");
                
                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Alterar(TarefaViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Tarefa  = _unitOfWork.TarefaRepository.Get(obj.Tarefa.Id);
                    
                    ViewModel.SelectProjetos = new SelectList(_unitOfWork.ProjetoRepository.All(), "Id", "Nome");
                    ViewModel.SelectSistemas = new SelectList(_unitOfWork.SistemaRepository.All(), "Id", "Nome");
                    ViewModel.SelectWorkflows = new SelectList(_unitOfWork.WorkflowRepository.All(), "Id", "Nome");
                    ViewModel.SelectTipoTarefas = new SelectList(_unitOfWork.TipoTarefaRepository.All(), "Id", "Nome");

                    return View(ViewModel);
                }

                _unitOfWork.TarefaRepository.Update(obj.Tarefa);

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
                ViewModel.Tarefa  = _unitOfWork.TarefaRepository.Get(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public IActionResult Remover(TarefaViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Tarefa  = _unitOfWork.TarefaRepository.Get(obj.Tarefa.Id);

                    return View(ViewModel);
                }

                _unitOfWork.TarefaRepository.Remove(obj.Tarefa.Id);

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
