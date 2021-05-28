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
    public class ImpedimentoTarefaController : Controller
    {
        private readonly IImpedimentoTarefaAppService _impedimentoTarefaAppService;
        private readonly ITarefaAppService _tarefaAppService;
        private readonly IImpedimentoAppService _impedimentoAppService;

        private ImpedimentoTarefaView _impedimentoTarefaView;

        public ImpedimentoTarefaController(IImpedimentoTarefaAppService impedimentoTarefaAppService,
                                           ITarefaAppService tarefaAppService,
                                           IImpedimentoAppService impedimentoAppService)
        {
            _impedimentoTarefaAppService = impedimentoTarefaAppService;
            _tarefaAppService = tarefaAppService;
            _impedimentoAppService = impedimentoAppService;
        }

        public ImpedimentoTarefaView ImpedimentoTarefaView
        {
            get
            {
                if (_impedimentoTarefaView == null)
                {
                    _impedimentoTarefaView = new ImpedimentoTarefaView();
                }

                return _impedimentoTarefaView;
            }
            set => _impedimentoTarefaView = value;
        }

        [HttpGet]
        public async Task<IActionResult> Listar(Guid idTarefa)
        {
            try
            {
                ImpedimentoTarefaView.Lista = await _impedimentoTarefaAppService.GetByTarefaAsync(idTarefa);

                ViewData["idTarefa"] = idTarefa;

                return View(ImpedimentoTarefaView);
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
            ImpedimentoTarefaView.Tarefa = await _tarefaAppService.GetAsync(idTarefa);
            ImpedimentoTarefaView.SelectImpedimentos = new SelectList(await _impedimentoAppService.AllAsync(), "Id", "Nome");

            return View(ImpedimentoTarefaView);
        }

        [HttpPost]
        public async Task<IActionResult> Incluir(ImpedimentoTarefaView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ImpedimentoTarefaView.Tarefa = await _tarefaAppService.GetAsync(obj.Tarefa.Id);
                    ImpedimentoTarefaView.SelectImpedimentos = new SelectList(await _impedimentoAppService.AllAsync(), "Id", "Nome");

                    return View(ImpedimentoTarefaView);
                }

                await _impedimentoTarefaAppService.AddAsync(obj.ImpedimentoTarefa);
                await _impedimentoTarefaAppService.SaveChangesAsync();

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
                ImpedimentoTarefaView.ImpedimentoTarefa = await _impedimentoTarefaAppService.GetAsync(id);
                ImpedimentoTarefaView.SelectImpedimentos = new SelectList(await _impedimentoAppService.AllAsync(), "Id", "Nome");

                return View(ImpedimentoTarefaView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(ImpedimentoTarefaView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ImpedimentoTarefaView.ImpedimentoTarefa = await _impedimentoTarefaAppService.GetAsync(obj.ImpedimentoTarefa.Id);
                    ImpedimentoTarefaView.SelectImpedimentos = new SelectList(await _impedimentoAppService.AllAsync(), "Id", "Nome");

                    return View(ImpedimentoTarefaView);
                }

                _impedimentoTarefaAppService.Update(obj.ImpedimentoTarefa);
                await _impedimentoTarefaAppService.SaveChangesAsync();

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
                ImpedimentoTarefaView.ImpedimentoTarefa = await _impedimentoTarefaAppService.GetAsync(id);

                return View(ImpedimentoTarefaView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remover(ImpedimentoTarefaView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ImpedimentoTarefaView.ImpedimentoTarefa = await _impedimentoTarefaAppService.GetAsync(obj.ImpedimentoTarefa.Id);

                    return View(ImpedimentoTarefaView);
                }

                await _impedimentoTarefaAppService.RemoveAsync(obj.ImpedimentoTarefa.Id);
                await _impedimentoTarefaAppService.SaveChangesAsync();

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
