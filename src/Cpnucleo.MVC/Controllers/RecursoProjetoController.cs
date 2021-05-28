using Cpnucleo.Application.Interfaces;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
{
    [Authorize]
    public class RecursoProjetoController : Controller
    {
        private readonly IRecursoProjetoAppService _recursoProjetoAppService;
        private readonly IRecursoAppService _recursoAppService;
        private readonly IProjetoAppService _projetoAppService;

        private RecursoProjetoView _recursoProjetoView;

        public RecursoProjetoController(IRecursoProjetoAppService recursoProjetoAppService,
                                        IRecursoAppService recursoAppService,
                                        IProjetoAppService projetoAppService)
        {
            _recursoProjetoAppService = recursoProjetoAppService;
            _recursoAppService = recursoAppService;
            _projetoAppService = projetoAppService;
        }

        public RecursoProjetoView RecursoProjetoView
        {
            get
            {
                if (_recursoProjetoView == null)
                {
                    _recursoProjetoView = new RecursoProjetoView();
                }

                return _recursoProjetoView;
            }
            set => _recursoProjetoView = value;
        }

        [HttpGet]
        public async Task<IActionResult> Listar(Guid idProjeto)
        {
            try
            {
                RecursoProjetoView.Lista = await _recursoProjetoAppService.GetByProjetoAsync(idProjeto);

                ViewData["idProjeto"] = idProjeto;

                return View(RecursoProjetoView);
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
            RecursoProjetoView.Projeto = await _projetoAppService.GetAsync(idProjeto);
            RecursoProjetoView.SelectRecursos = new SelectList(await _recursoAppService.AllAsync(), "Id", "Nome");

            return View(RecursoProjetoView);
        }

        [HttpPost]
        public async Task<IActionResult> Incluir(RecursoProjetoView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    RecursoProjetoView.Projeto = await _projetoAppService.GetAsync(obj.Projeto.Id);
                    RecursoProjetoView.SelectRecursos = new SelectList(await _recursoAppService.AllAsync(), "Id", "Nome");

                    return View(RecursoProjetoView);
                }

                await _recursoProjetoAppService.AddAsync(obj.RecursoProjeto);
                await _recursoProjetoAppService.SaveChangesAsync();

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
                RecursoProjetoView.RecursoProjeto = await _recursoProjetoAppService.GetAsync(id);
                RecursoProjetoView.SelectRecursos = new SelectList(await _recursoAppService.AllAsync(), "Id", "Nome");

                return View(RecursoProjetoView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(RecursoProjetoView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    RecursoProjetoView.RecursoProjeto = await _recursoProjetoAppService.GetAsync(obj.RecursoProjeto.Id);
                    RecursoProjetoView.SelectRecursos = new SelectList(await _recursoAppService.AllAsync(), "Id", "Nome");

                    return View(RecursoProjetoView);
                }

                _recursoProjetoAppService.Update(obj.RecursoProjeto);
                await _recursoProjetoAppService.SaveChangesAsync();

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
                RecursoProjetoView.RecursoProjeto = await _recursoProjetoAppService.GetAsync(id);

                return View(RecursoProjetoView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remover(RecursoProjetoView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    RecursoProjetoView.RecursoProjeto = await _recursoProjetoAppService.GetAsync(obj.RecursoProjeto.Id);

                    return View(RecursoProjetoView);
                }

                await _recursoProjetoAppService.RemoveAsync(obj.RecursoProjeto.Id);
                await _recursoProjetoAppService.SaveChangesAsync();

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
