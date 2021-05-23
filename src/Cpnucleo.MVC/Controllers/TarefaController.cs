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
    public class TarefaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TarefaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private TarefaView _viewModel;

        public TarefaView ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = new TarefaView();

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
                ViewModel.Lista = await _unitOfWork.TarefaRepository.AllAsync(true);

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
            ViewModel.SelectProjetos = new SelectList(await _unitOfWork.ProjetoRepository.AllAsync(), "Id", "Nome");
            ViewModel.SelectSistemas = new SelectList(await _unitOfWork.SistemaRepository.AllAsync(), "Id", "Nome");
            ViewModel.SelectWorkflows = new SelectList(await _unitOfWork.WorkflowRepository.AllAsync(), "Id", "Nome");
            ViewModel.SelectTipoTarefas = new SelectList(await _unitOfWork.TipoTarefaRepository.AllAsync(), "Id", "Nome");

            ViewModel.User = HttpContext.User;

            return View(ViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Incluir(TarefaView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.SelectProjetos = new SelectList(await _unitOfWork.ProjetoRepository.AllAsync(), "Id", "Nome");
                    ViewModel.SelectSistemas = new SelectList(await _unitOfWork.SistemaRepository.AllAsync(), "Id", "Nome");
                    ViewModel.SelectWorkflows = new SelectList(await _unitOfWork.WorkflowRepository.AllAsync(), "Id", "Nome");
                    ViewModel.SelectTipoTarefas = new SelectList(await _unitOfWork.TipoTarefaRepository.AllAsync(), "Id", "Nome");
                    
                    ViewModel.User = HttpContext.User;  

                    return View(ViewModel);
                }

                await _unitOfWork.TarefaRepository.AddAsync(obj.Tarefa);

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
                ViewModel.Tarefa  = await _unitOfWork.TarefaRepository.GetAsync(id);
                
                ViewModel.SelectProjetos = new SelectList(await _unitOfWork.ProjetoRepository.AllAsync(), "Id", "Nome");
                ViewModel.SelectSistemas = new SelectList(await _unitOfWork.SistemaRepository.AllAsync(), "Id", "Nome");
                ViewModel.SelectWorkflows = new SelectList(await _unitOfWork.WorkflowRepository.AllAsync(), "Id", "Nome");
                ViewModel.SelectTipoTarefas = new SelectList(await _unitOfWork.TipoTarefaRepository.AllAsync(), "Id", "Nome");
                
                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(TarefaView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Tarefa  = await _unitOfWork.TarefaRepository.GetAsync(obj.Tarefa.Id);
                    
                    ViewModel.SelectProjetos = new SelectList(await _unitOfWork.ProjetoRepository.AllAsync(), "Id", "Nome");
                    ViewModel.SelectSistemas = new SelectList(await _unitOfWork.SistemaRepository.AllAsync(), "Id", "Nome");
                    ViewModel.SelectWorkflows = new SelectList(await _unitOfWork.WorkflowRepository.AllAsync(), "Id", "Nome");
                    ViewModel.SelectTipoTarefas = new SelectList(await _unitOfWork.TipoTarefaRepository.AllAsync(), "Id", "Nome");

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
        public async Task<IActionResult> Remover(Guid id)
        {
            try
            {
                ViewModel.Tarefa  = await _unitOfWork.TarefaRepository.GetAsync(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remover(TarefaView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Tarefa  = await _unitOfWork.TarefaRepository.GetAsync(obj.Tarefa.Id);

                    return View(ViewModel);
                }

                await _unitOfWork.TarefaRepository.RemoveAsync(obj.Tarefa.Id);

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
