using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.CreateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.RemoveApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.UpdateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetByRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.ListTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.GetWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.ListWorkflow;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.MVC.Models;
using Cpnucleo.MVC.Services;
using MagicOnion.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
{
    [Authorize]
    public class ApontamentoController : BaseController
    {
        private readonly IApontamentoGrpcService _apontamentoGrpcService;
        private readonly ITarefaGrpcService _tarefaGrpcService;
        private readonly IWorkflowGrpcService _workflowGrpcService;

        private ApontamentoView _apontamentoView;

        public ApontamentoController(IConfiguration configuration)
            : base(configuration)
        {
            _apontamentoGrpcService = MagicOnionClient.Create<IApontamentoGrpcService>(CreateAuthenticatedChannel());
            _tarefaGrpcService = MagicOnionClient.Create<ITarefaGrpcService>(CreateAuthenticatedChannel());
            _workflowGrpcService = MagicOnionClient.Create<IWorkflowGrpcService>(CreateAuthenticatedChannel());
        }

        public ApontamentoView ApontamentoView
        {
            get
            {
                if (_apontamentoView == null)
                {
                    _apontamentoView = new ApontamentoView();
                }

                return _apontamentoView;
            }
            set => _apontamentoView = value;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                string retorno = ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
                Guid idRecurso = new(retorno);

                GetByRecursoResponse response = await _apontamentoGrpcService.GetByRecursoAsync(new GetByRecursoQuery { IdRecurso = idRecurso });
                ApontamentoView.Lista = response.Apontamentos;

                await CarregarTarefasByRecurso(idRecurso);

                return View(ApontamentoView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Listar(ApontamentoView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    string retorno = ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
                    Guid idRecurso = new(retorno);

                    GetByRecursoResponse response = await _apontamentoGrpcService.GetByRecursoAsync(new GetByRecursoQuery { IdRecurso = idRecurso });
                    ApontamentoView.Lista = response.Apontamentos;

                    await CarregarTarefasByRecurso(idRecurso);

                    return View(ApontamentoView);
                }

                await _apontamentoGrpcService.AddAsync(new CreateApontamentoCommand { Apontamento = obj.Apontamento });

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
                GetApontamentoResponse response = await _apontamentoGrpcService.GetAsync(new GetApontamentoQuery { Id = id });
                ApontamentoView.Apontamento = response.Apontamento;

                return View(ApontamentoView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remover(ApontamentoView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    GetApontamentoResponse response = await _apontamentoGrpcService.GetAsync(new GetApontamentoQuery { Id = obj.Apontamento.Id });
                    ApontamentoView.Apontamento = response.Apontamento;

                    return View(ApontamentoView);
                }

                await _apontamentoGrpcService.RemoveAsync(new RemoveApontamentoCommand { Id = obj.Apontamento.Id });

                return RedirectToAction("Listar");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> FluxoTrabalho()
        {
            try
            {
                await CarregarWorkflows();
                await CarregarTarefas();

                return View(ApontamentoView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<JsonResult> FluxoTrabalho(Guid idTarefa, Guid idWorkflow)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await CarregarWorkflows();
                    await CarregarTarefas();

                    return Json(new { success = false, message = "", body = ApontamentoView });
                }

                GetTarefaResponse getTarefaResponse = await _tarefaGrpcService.GetAsync(new GetTarefaQuery { Id = idTarefa });
                TarefaViewModel tarefa = getTarefaResponse.Tarefa;

                GetWorkflowResponse getWorkflowResponse = await _workflowGrpcService.GetAsync(new GetWorkflowQuery { Id = idWorkflow });
                WorkflowViewModel workflow = getWorkflowResponse.Workflow;

                tarefa.IdWorkflow = workflow.Id;
                tarefa.Workflow = workflow;

                await _tarefaGrpcService.UpdateAsync(new UpdateTarefaCommand { Tarefa = tarefa });

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        private async Task CarregarTarefasByRecurso(Guid idRecurso)
        {
            Infra.CrossCutting.Util.Queries.Tarefa.GetByRecurso.GetByRecursoResponse response = await _tarefaGrpcService.GetByRecursoAsync(new Infra.CrossCutting.Util.Queries.Tarefa.GetByRecurso.GetByRecursoQuery { IdRecurso = idRecurso });
            ApontamentoView.ListaTarefas = response.Tarefas;
        }

        private async Task CarregarWorkflows()
        {
            ListWorkflowResponse response = await _workflowGrpcService.AllAsync(new ListWorkflowQuery { });
            ApontamentoView.ListaWorkflow = response.Workflows;
        }

        private async Task CarregarTarefas()
        {
            ListTarefaResponse response = await _tarefaGrpcService.AllAsync(new ListTarefaQuery { GetDependencies = true });
            ApontamentoView.ListaTarefas = response.Tarefas;
        }
    }
}
