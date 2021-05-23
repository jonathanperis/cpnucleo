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
        public async Task<IActionResult> Listar(Guid idProjeto)
        {
            try
            {
                ViewModel.Lista = await _unitOfWork.RecursoProjetoRepository.GetByProjetoAsync(idProjeto);

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
        public async Task<IActionResult> Incluir(Guid idProjeto)
        {
            ViewModel.Projeto = await _unitOfWork.ProjetoRepository.GetAsync(idProjeto);
            ViewModel.SelectRecursos = new SelectList(await _unitOfWork.RecursoRepository.AllAsync(), "Id", "Nome");

            return View(ViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Incluir(RecursoProjetoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Projeto = await _unitOfWork.ProjetoRepository.GetAsync(obj.Projeto.Id);
                    ViewModel.SelectRecursos = new SelectList(await _unitOfWork.RecursoRepository.AllAsync(), "Id", "Nome");

                    return View(ViewModel);
                }

                await _unitOfWork.RecursoProjetoRepository.AddAsync(obj.RecursoProjeto);

                return RedirectToAction("Listar", new { idProjeto = obj.RecursoProjeto.IdProjeto });
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
                ViewModel.RecursoProjeto  = await _unitOfWork.RecursoProjetoRepository.GetAsync(id);
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
        public async Task<IActionResult> Alterar(RecursoProjetoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.RecursoProjeto  = await _unitOfWork.RecursoProjetoRepository.GetAsync(obj.RecursoProjeto.Id);
                    ViewModel.SelectRecursos = new SelectList(await _unitOfWork.RecursoRepository.AllAsync(), "Id", "Nome");

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
        public async Task<IActionResult> Remover(Guid id)
        {
            try
            {
                ViewModel.RecursoProjeto  = await _unitOfWork.RecursoProjetoRepository.GetAsync(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remover(RecursoProjetoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.RecursoProjeto  = await _unitOfWork.RecursoProjetoRepository.GetAsync(obj.RecursoProjeto.Id);

                    return View(ViewModel);
                }

                await _unitOfWork.RecursoProjetoRepository.RemoveAsync(obj.RecursoProjeto.Id);

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
