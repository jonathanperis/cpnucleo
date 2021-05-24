using System;
using Microsoft.AspNetCore.Mvc;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Cpnucleo.MVC.Services;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.Application.Interfaces;

namespace Cpnucleo.MVC.Controllers
{
    [Authorize]
    public class ApontamentoController : Controller
    {
        private readonly IApontamentoAppService _apontamentoAppService;
        private readonly ITarefaAppService _tarefaAppService;
        private readonly IWorkflowAppService _workflowAppService;
        private readonly IImpedimentoTarefaAppService _impedimentoTarefaAppService;

        private ApontamentoView _apontamentoView;

        public ApontamentoController(IApontamentoAppService apontamentoAppService, 
                                     ITarefaAppService tarefaAppService, 
                                     IWorkflowAppService workflowAppService,
                                     IImpedimentoTarefaAppService impedimentoTarefaAppService)
        {
            _apontamentoAppService = apontamentoAppService;
            _tarefaAppService = tarefaAppService;
            _workflowAppService = workflowAppService;
            _impedimentoTarefaAppService = impedimentoTarefaAppService;
        }

        public ApontamentoView ApontamentoView
        {
            get
            {
                if (_apontamentoView == null)
                    _apontamentoView = new ApontamentoView();

                return _apontamentoView;
            }
            set
            {
                _apontamentoView = value;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                string retorno = ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
                Guid idRecurso = new(retorno);

                ApontamentoView.Lista = await _apontamentoAppService.GetByRecursoAsync(idRecurso);
                ApontamentoView.ListaTarefas = await _tarefaAppService.GetByRecursoAsync(idRecurso);

                await PreencherDadosAdicionais(ApontamentoView.ListaTarefas);

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

                    ApontamentoView.Lista = await _apontamentoAppService.GetByRecursoAsync(idRecurso);
                    ApontamentoView.ListaTarefas = await _tarefaAppService.GetByRecursoAsync(idRecurso);

                    await PreencherDadosAdicionais(ApontamentoView.ListaTarefas);

                    return View(ApontamentoView);
                }

                await _apontamentoAppService.AddAsync(obj.Apontamento);

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
                ApontamentoView.Apontamento  = await _apontamentoAppService.GetAsync(id);

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
                    ApontamentoView.Apontamento  = await _apontamentoAppService.GetAsync(obj.Apontamento.Id);

                    return View(ApontamentoView);
                }

                await _apontamentoAppService.RemoveAsync(obj.Apontamento.Id);

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
                ApontamentoView.ListaWorkflow = await _workflowAppService.AllAsync();
                ApontamentoView.ListaTarefas = await _tarefaAppService.AllAsync(true);

                int colunas = await _workflowAppService.GetQuantidadeColunasAsync();

                foreach (WorkflowViewModel item in ApontamentoView.ListaWorkflow)
                {
                    item.TamanhoColuna = _workflowAppService.GetTamanhoColuna(colunas);
                }                

                await PreencherDadosAdicionais(ApontamentoView.ListaTarefas);

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
                    ApontamentoView.ListaWorkflow = await _workflowAppService.AllAsync();
                    ApontamentoView.ListaTarefas = await _tarefaAppService.AllAsync(true);

                    await PreencherDadosAdicionais (ApontamentoView.ListaTarefas);

                    return Json(new { success = false, message = "", body = ApontamentoView });
                }

                TarefaViewModel tarefa = await _tarefaAppService.GetAsync(idTarefa);
                WorkflowViewModel workflow = await _workflowAppService.GetAsync(idWorkflow);

                tarefa.IdWorkflow = workflow.Id;
                tarefa.Workflow = workflow;

                _tarefaAppService.Update(tarefa);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }    

        private async Task<IEnumerable<TarefaViewModel>> PreencherDadosAdicionais(IEnumerable<TarefaViewModel> lista)
        {
            int colunas = await _workflowAppService.GetQuantidadeColunasAsync();

            foreach (TarefaViewModel item in lista)
            {
                item.Workflow.TamanhoColuna = _workflowAppService.GetTamanhoColuna(colunas);
                
                item.HorasConsumidas = await _apontamentoAppService.GetTotalHorasPorRecursoAsync(item.IdRecurso, item.Id);
                item.HorasRestantes = item.QtdHoras - item.HorasConsumidas;

                IEnumerable<ImpedimentoTarefaViewModel> tarefas = await _impedimentoTarefaAppService.GetByTarefaAsync(item.Id);

                if (tarefas.Any())
                {
                    item.TipoTarefa.Element = "warning-element";
                }
                else if (DateTime.Now.Date >= item.DataInicio && DateTime.Now.Date <= item.DataTermino)
                {
                    item.TipoTarefa.Element = "success-element";
                }
                else if (DateTime.Now.Date > item.DataTermino)
                {
                    item.TipoTarefa.Element = "danger-element";
                }
                else
                {
                    item.TipoTarefa.Element = "info-element";
                }
            }

            return lista;
        }         
    }
}
