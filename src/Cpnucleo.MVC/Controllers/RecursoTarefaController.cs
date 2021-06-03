using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.CreateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.RemoveRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.UpdateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.ListRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetByTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetRecursoTarefa;
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
    public class RecursoTarefaController : BaseController
    {
        private readonly IRecursoTarefaService _recursoTarefaService;
        private readonly IRecursoService _recursoService;
        private readonly ITarefaService _TarefaService;

        private RecursoTarefaView _recursoTarefaView;

        public RecursoTarefaController(IRecursoTarefaService recursoTarefaService,
                                        IRecursoService recursoService,
                                        ITarefaService TarefaService)
        {
            _recursoTarefaService = recursoTarefaService;
            _recursoService = recursoService;
            _TarefaService = TarefaService;
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
                GetByTarefaResponse response = await _recursoTarefaService.GetByTarefaAsync(Token, new GetByTarefaQuery { IdTarefa = idTarefa });
                RecursoTarefaView.Lista = response.RecursoTarefas;

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
            await ObterTarefa(idTarefa);
            await CarregarSelectRecursos();

            return View(RecursoTarefaView);
        }

        [HttpPost]
        public async Task<IActionResult> Incluir(RecursoTarefaView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await ObterTarefa(obj.Tarefa.Id);
                    await CarregarSelectRecursos();

                    return View(RecursoTarefaView);
                }

                await _recursoTarefaService.AddAsync(Token, new CreateRecursoTarefaCommand { RecursoTarefa = obj.RecursoTarefa });

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
                GetRecursoTarefaResponse response = await _recursoTarefaService.GetAsync(Token, new GetRecursoTarefaQuery { Id = id });
                RecursoTarefaView.RecursoTarefa = response.RecursoTarefa;

                await CarregarSelectRecursos();

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
                    GetRecursoTarefaResponse response = await _recursoTarefaService.GetAsync(Token, new GetRecursoTarefaQuery { Id = obj.RecursoTarefa.Id });
                    RecursoTarefaView.RecursoTarefa = response.RecursoTarefa;

                    await CarregarSelectRecursos();

                    return View(RecursoTarefaView);
                }

                await _recursoTarefaService.UpdateAsync(Token, new UpdateRecursoTarefaCommand { RecursoTarefa = obj.RecursoTarefa });

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
                GetRecursoTarefaResponse response = await _recursoTarefaService.GetAsync(Token, new GetRecursoTarefaQuery { Id = id });
                RecursoTarefaView.RecursoTarefa = response.RecursoTarefa;

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
                    GetRecursoTarefaResponse response = await _recursoTarefaService.GetAsync(Token, new GetRecursoTarefaQuery { Id = obj.RecursoTarefa.Id });
                    RecursoTarefaView.RecursoTarefa = response.RecursoTarefa;

                    return View(RecursoTarefaView);
                }

                await _recursoTarefaService.RemoveAsync(Token, new RemoveRecursoTarefaCommand { Id = obj.RecursoTarefa.Id });

                return RedirectToAction("Listar", new { idTarefa = obj.RecursoTarefa.IdTarefa });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        private async Task ObterTarefa(Guid idTarefa)
        {
            GetTarefaResponse response = await _TarefaService.GetAsync(Token, new GetTarefaQuery { Id = idTarefa });
            RecursoTarefaView.Tarefa = response.Tarefa;
        }

        private async Task CarregarSelectRecursos()
        {
            ListRecursoResponse response = await _recursoService.AllAsync(Token, new ListRecursoQuery { });
            RecursoTarefaView.SelectRecursos = new SelectList(response.Recursos, "Id", "Nome");
        }
    }
}
