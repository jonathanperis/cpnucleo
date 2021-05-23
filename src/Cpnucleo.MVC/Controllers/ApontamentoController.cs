using System;
using Cpnucleo.Domain.UoW;
using Microsoft.AspNetCore.Mvc;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Cpnucleo.MVC.Services;
using System.Security.Claims;
using System.Collections.Generic;
using Cpnucleo.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
{
    [Authorize]
    public class ApontamentoController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApontamentoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private ApontamentoViewModel _viewModel;

        public ApontamentoViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = new ApontamentoViewModel();

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
                string retorno = ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
                Guid idRecurso = new(retorno);

                ViewModel.Lista = await _unitOfWork.ApontamentoRepository.GetByRecursoAsync(idRecurso);
                ViewModel.ListaTarefas = await _unitOfWork.TarefaRepository.GetByRecursoAsync(idRecurso);

                await PreencherDadosAdicionais(ViewModel.ListaTarefas);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Listar(ApontamentoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    string retorno = ClaimsService.ReadClaimsPrincipal(HttpContext.User, ClaimTypes.PrimarySid);
                    Guid idRecurso = new(retorno);

                    ViewModel.Lista = await _unitOfWork.ApontamentoRepository.GetByRecursoAsync(idRecurso);
                    ViewModel.ListaTarefas = await _unitOfWork.TarefaRepository.GetByRecursoAsync(idRecurso);

                    await PreencherDadosAdicionais(ViewModel.ListaTarefas);

                    return View(ViewModel);
                }

                await _unitOfWork.ApontamentoRepository.AddAsync(obj.Apontamento);

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
                ViewModel.Apontamento  = await _unitOfWork.ApontamentoRepository.GetAsync(id);

                return View(ViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remover(ApontamentoViewModel obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewModel.Apontamento  = await _unitOfWork.ApontamentoRepository.GetAsync(obj.Apontamento.Id);

                    return View(ViewModel);
                }

                await _unitOfWork.ApontamentoRepository.RemoveAsync(obj.Apontamento.Id);

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
                ViewModel.ListaWorkflow = await _unitOfWork.WorkflowRepository.AllAsync();
                ViewModel.ListaTarefas = await _unitOfWork.TarefaRepository.AllAsync(true);

                int colunas = await _unitOfWork.WorkflowRepository.GetQuantidadeColunasAsync();

                foreach (Workflow item in ViewModel.ListaWorkflow)
                {
                    item.TamanhoColuna = _unitOfWork.WorkflowRepository.GetTamanhoColuna(colunas);
                }                

                await PreencherDadosAdicionais(ViewModel.ListaTarefas);

                return View(ViewModel);
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
                    ViewModel.ListaWorkflow = await _unitOfWork.WorkflowRepository.AllAsync();
                    ViewModel.ListaTarefas = await _unitOfWork.TarefaRepository.AllAsync(true);

                    await PreencherDadosAdicionais (ViewModel.ListaTarefas);

                    return Json(new { success = false, message = "", body = ViewModel });
                }

                Tarefa tarefa = await _unitOfWork.TarefaRepository.GetAsync(idTarefa);
                Workflow workflow = await _unitOfWork.WorkflowRepository.GetAsync(idWorkflow);

                tarefa.IdWorkflow = workflow.Id;
                tarefa.Workflow = workflow;

                _unitOfWork.TarefaRepository.Update(tarefa);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }    

        private async Task<IEnumerable<Tarefa>> PreencherDadosAdicionais(IEnumerable<Tarefa> lista)
        {
            int colunas = await _unitOfWork.WorkflowRepository.GetQuantidadeColunasAsync();

            foreach (Tarefa item in lista)
            {
                item.Workflow.TamanhoColuna = _unitOfWork.WorkflowRepository.GetTamanhoColuna(colunas);
                
                item.HorasConsumidas = await _unitOfWork.ApontamentoRepository.GetTotalHorasPorRecursoAsync(item.IdRecurso, item.Id);
                item.HorasRestantes = item.QtdHoras - item.HorasConsumidas;

                IEnumerable<ImpedimentoTarefa> tarefas = await _unitOfWork.ImpedimentoTarefaRepository.GetByTarefaAsync(item.Id);

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
