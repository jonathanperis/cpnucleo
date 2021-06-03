using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.CreateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.RemoveTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.UpdateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.ListProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.ListSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.ListTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.ListTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.ListWorkflow;
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
    public class TarefaController : BaseController
    {
        private readonly ITarefaService _tarefaService;
        private readonly ISistemaService _sistemaService;
        private readonly IProjetoService _projetoService;
        private readonly IWorkflowService _workflowService;
        private readonly ITipoTarefaService _tipoTarefaService;

        private TarefaView _tarefaView;

        public TarefaController(ITarefaService tarefaService,
                                ISistemaService sistemaService,
                                IProjetoService projetoService,
                                IWorkflowService workflowService,
                                ITipoTarefaService tipoTarefaService)
        {
            _tarefaService = tarefaService;
            _sistemaService = sistemaService;
            _projetoService = projetoService;
            _workflowService = workflowService;
            _tipoTarefaService = tipoTarefaService;
        }

        public TarefaView TarefaView
        {
            get
            {
                if (_tarefaView == null)
                {
                    _tarefaView = new TarefaView();
                }

                return _tarefaView;
            }
            set => _tarefaView = value;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                ListTarefaResponse response = await _tarefaService.AllAsync(Token, new ListTarefaQuery { GetDependencies = true });
                TarefaView.Lista = response.Tarefas;

                return View(TarefaView);
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
            await CarregarSelectSistemas();
            await CarregarSelectProjetos();
            await CarregarSelectWorkflows();
            await CarregarSelectTipoTarefas();

            TarefaView.User = HttpContext.User;

            return View(TarefaView);
        }

        [HttpPost]
        public async Task<IActionResult> Incluir(TarefaView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await CarregarSelectSistemas();
                    await CarregarSelectProjetos();
                    await CarregarSelectWorkflows();
                    await CarregarSelectTipoTarefas();

                    TarefaView.User = HttpContext.User;

                    return View(TarefaView);
                }

                await _tarefaService.AddAsync(Token, new CreateTarefaCommand { Tarefa = obj.Tarefa });

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
                GetTarefaResponse response = await _tarefaService.GetAsync(Token, new GetTarefaQuery { Id = id });
                TarefaView.Tarefa = response.Tarefa;

                await CarregarSelectSistemas();
                await CarregarSelectProjetos();
                await CarregarSelectWorkflows();
                await CarregarSelectTipoTarefas();

                return View(TarefaView);
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
                    GetTarefaResponse response = await _tarefaService.GetAsync(Token, new GetTarefaQuery { Id = obj.Tarefa.Id });
                    TarefaView.Tarefa = response.Tarefa;

                    await CarregarSelectSistemas();
                    await CarregarSelectProjetos();
                    await CarregarSelectWorkflows();
                    await CarregarSelectTipoTarefas();

                    return View(TarefaView);
                }

                await _tarefaService.UpdateAsync(Token, new UpdateTarefaCommand { Tarefa = obj.Tarefa });

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
                GetTarefaResponse response = await _tarefaService.GetAsync(Token, new GetTarefaQuery { Id = id });
                TarefaView.Tarefa = response.Tarefa;

                return View(TarefaView);
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
                    GetTarefaResponse response = await _tarefaService.GetAsync(Token, new GetTarefaQuery { Id = obj.Tarefa.Id });
                    TarefaView.Tarefa = response.Tarefa;

                    return View(TarefaView);
                }

                await _tarefaService.RemoveAsync(Token, new RemoveTarefaCommand { Id = obj.Tarefa.Id });

                return RedirectToAction("Listar");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        private async Task CarregarSelectSistemas()
        {
            ListSistemaResponse response = await _sistemaService.AllAsync(Token, new ListSistemaQuery { });
            TarefaView.SelectSistemas = new SelectList(response.Sistemas, "Id", "Nome");
        }

        private async Task CarregarSelectProjetos()
        {
            ListProjetoResponse response = await _projetoService.AllAsync(Token, new ListProjetoQuery { });
            TarefaView.SelectProjetos = new SelectList(response.Projetos, "Id", "Nome");
        }

        private async Task CarregarSelectWorkflows()
        {
            ListWorkflowResponse response = await _workflowService.AllAsync(Token, new ListWorkflowQuery { });
            TarefaView.SelectWorkflows = new SelectList(response.Workflows, "Id", "Nome");
        }

        private async Task CarregarSelectTipoTarefas()
        {
            ListTipoTarefaResponse response = await _tipoTarefaService.AllAsync(Token, new ListTipoTarefaQuery { });
            TarefaView.SelectTipoTarefas = new SelectList(response.TipoTarefas, "Id", "Nome");
        }
    }
}
