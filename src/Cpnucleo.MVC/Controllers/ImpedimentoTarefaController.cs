using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.CreateImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.RemoveImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.UpdateImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.ListImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.GetByTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa.GetImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetTarefa;
using Cpnucleo.MVC.Interfaces;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
{
    [Authorize]
    public class ImpedimentoTarefaController : BaseController
    {
        private readonly IImpedimentoTarefaService _impedimentoTarefaService;
        private readonly ITarefaService _tarefaService;
        private readonly IImpedimentoService _impedimentoService;

        private ImpedimentoTarefaView _impedimentoTarefaView;

        public ImpedimentoTarefaController(IImpedimentoTarefaService impedimentoTarefaService,
                                           ITarefaService tarefaService,
                                           IImpedimentoService impedimentoService)
        {
            _impedimentoTarefaService = impedimentoTarefaService;
            _tarefaService = tarefaService;
            _impedimentoService = impedimentoService;
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
                GetByTarefaResponse response = await _impedimentoTarefaService.GetByTarefaAsync(Token, new GetByTarefaQuery { IdTarefa = idTarefa });
                ImpedimentoTarefaView.Lista = response.ImpedimentoTarefas;

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
            await ObterTarefa(idTarefa);
            await CarregarSelectImpedimentos();

            return View(ImpedimentoTarefaView);
        }

        [HttpPost]
        public async Task<IActionResult> Incluir(ImpedimentoTarefaView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await ObterTarefa(obj.Tarefa.Id);
                    await CarregarSelectImpedimentos();

                    return View(ImpedimentoTarefaView);
                }

                await _impedimentoTarefaService.AddAsync(Token, new CreateImpedimentoTarefaCommand { ImpedimentoTarefa = obj.ImpedimentoTarefa });

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
                GetImpedimentoTarefaResponse response = await _impedimentoTarefaService.GetAsync(Token, new GetImpedimentoTarefaQuery { Id = id });
                ImpedimentoTarefaView.ImpedimentoTarefa = response.ImpedimentoTarefa;

                await CarregarSelectImpedimentos();

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
                    GetImpedimentoTarefaResponse response = await _impedimentoTarefaService.GetAsync(Token, new GetImpedimentoTarefaQuery { Id = obj.ImpedimentoTarefa.Id });
                    ImpedimentoTarefaView.ImpedimentoTarefa = response.ImpedimentoTarefa;

                    await CarregarSelectImpedimentos();

                    return View(ImpedimentoTarefaView);
                }

                await _impedimentoTarefaService.UpdateAsync(Token, new UpdateImpedimentoTarefaCommand { ImpedimentoTarefa = obj.ImpedimentoTarefa });

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
                GetImpedimentoTarefaResponse response = await _impedimentoTarefaService.GetAsync(Token, new GetImpedimentoTarefaQuery { Id = id });
                ImpedimentoTarefaView.ImpedimentoTarefa = response.ImpedimentoTarefa;

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
                    GetImpedimentoTarefaResponse response = await _impedimentoTarefaService.GetAsync(Token, new GetImpedimentoTarefaQuery { Id = obj.ImpedimentoTarefa.Id });
                    ImpedimentoTarefaView.ImpedimentoTarefa = response.ImpedimentoTarefa;

                    return View(ImpedimentoTarefaView);
                }

                await _impedimentoTarefaService.RemoveAsync(Token, new RemoveImpedimentoTarefaCommand { Id = obj.ImpedimentoTarefa.Id });

                return RedirectToAction("Listar", new { idTarefa = obj.ImpedimentoTarefa.IdTarefa });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        private async Task ObterTarefa(Guid idTarefa)
        {
            GetTarefaResponse response = await _tarefaService.GetAsync(Token, new GetTarefaQuery { Id = idTarefa });
            ImpedimentoTarefaView.Tarefa = response.Tarefa;
        }

        private async Task CarregarSelectImpedimentos()
        {
            ListImpedimentoResponse response = await _impedimentoService.AllAsync(Token, new ListImpedimentoQuery { });
            ImpedimentoTarefaView.SelectImpedimentos = new SelectList(response.Impedimentos, "Id", "Nome");
        }
    }
}
