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
    public class RecursoTarefaController : Controller
    {
        private readonly IRecursoTarefaAppService _recursoTarefaAppService;
        private readonly IRecursoAppService _recursoAppService;
        private readonly ITarefaAppService _tarefaAppService;

        private RecursoTarefaView _recursoTarefaView;

        public RecursoTarefaController(IRecursoTarefaAppService recursoTarefaAppService,
                                       IRecursoAppService recursoAppService,
                                       ITarefaAppService tarefaAppService)
        {
            _recursoTarefaAppService = recursoTarefaAppService;
            _recursoAppService = recursoAppService;
            _tarefaAppService = tarefaAppService;
        }

        public RecursoTarefaView RecursoTarefaView
        {
            get
            {
                if (_recursoTarefaView == null)
                {
                    _recursoTarefaView = new RecursoTarefaView();
                }

                return _recursoTarefaView;
            }
            set => _recursoTarefaView = value;
        }

        [HttpGet]
        public async Task<IActionResult> Listar(Guid idTarefa)
        {
            try
            {
                RecursoTarefaView.Lista = await _recursoTarefaAppService.GetByTarefaAsync(idTarefa);

                ViewData["idTarefa"] = idTarefa;

                return View(RecursoTarefaView);
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
            RecursoTarefaView.Tarefa = await _tarefaAppService.GetAsync(idTarefa);
            RecursoTarefaView.SelectRecursos = new SelectList(await _recursoAppService.AllAsync(), "Id", "Nome");

            return View(RecursoTarefaView);
        }

        [HttpPost]
        public async Task<IActionResult> Incluir(RecursoTarefaView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    RecursoTarefaView.Tarefa = await _tarefaAppService.GetAsync(obj.Tarefa.Id);
                    RecursoTarefaView.SelectRecursos = new SelectList(await _recursoAppService.AllAsync(), "Id", "Nome");

                    return View(RecursoTarefaView);
                }

                await _recursoTarefaAppService.AddAsync(obj.RecursoTarefa);
                await _recursoTarefaAppService.SaveChangesAsync();

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
                RecursoTarefaView.RecursoTarefa = await _recursoTarefaAppService.GetAsync(id);
                RecursoTarefaView.SelectRecursos = new SelectList(await _recursoAppService.AllAsync(), "Id", "Nome");

                return View(RecursoTarefaView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(RecursoTarefaView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    RecursoTarefaView.RecursoTarefa = await _recursoTarefaAppService.GetAsync(obj.RecursoTarefa.Id);
                    RecursoTarefaView.SelectRecursos = new SelectList(await _recursoAppService.AllAsync(), "Id", "Nome");

                    return View(RecursoTarefaView);
                }

                _recursoTarefaAppService.Update(obj.RecursoTarefa);
                await _recursoTarefaAppService.SaveChangesAsync();

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
                RecursoTarefaView.RecursoTarefa = await _recursoTarefaAppService.GetAsync(id);

                return View(RecursoTarefaView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remover(RecursoTarefaView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    RecursoTarefaView.RecursoTarefa = await _recursoTarefaAppService.GetAsync(obj.RecursoTarefa.Id);

                    return View(RecursoTarefaView);
                }

                await _recursoTarefaAppService.RemoveAsync(obj.RecursoTarefa.Id);
                await _recursoTarefaAppService.SaveChangesAsync();

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
