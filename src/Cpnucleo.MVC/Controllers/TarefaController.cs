using System;
using Microsoft.AspNetCore.Mvc;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Cpnucleo.Application.Interfaces;

namespace Cpnucleo.MVC.Controllers
{
    [Authorize]
    public class TarefaController : Controller
    {
        private readonly ITarefaAppService _tarefaAppService;
        private readonly ISistemaAppService _sistemaAppService;
        private readonly IProjetoAppService _projetoAppService;
        private readonly IWorkflowAppService _workflowAppService;
        private readonly ITipoTarefaAppService _tipoTarefaAppService;

        private TarefaView _tarefaView;

        public TarefaController(ITarefaAppService tarefaAppService, 
                                ISistemaAppService sistemaAppService, 
                                IProjetoAppService projetoAppService, 
                                IWorkflowAppService workflowAppService, 
                                ITipoTarefaAppService tipoTarefaAppService)
        {
            _tarefaAppService = tarefaAppService;
            _sistemaAppService = sistemaAppService;
            _projetoAppService = projetoAppService;
            _workflowAppService = workflowAppService;
            _tipoTarefaAppService = tipoTarefaAppService;
        }

        public TarefaView TarefaView
        {
            get
            {
                if (_tarefaView == null)
                    _tarefaView = new TarefaView();

                return _tarefaView;
            }
            set
            {
                _tarefaView = value;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                TarefaView.Lista = await _tarefaAppService.AllAsync(true);

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
            TarefaView.SelectSistemas = new SelectList(await _sistemaAppService.AllAsync(), "Id", "Nome");
            TarefaView.SelectProjetos = new SelectList(await _projetoAppService.AllAsync(), "Id", "Nome");
            TarefaView.SelectWorkflows = new SelectList(await _workflowAppService.AllAsync(), "Id", "Nome");
            TarefaView.SelectTipoTarefas = new SelectList(await _tipoTarefaAppService.AllAsync(), "Id", "Nome");

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
                    TarefaView.SelectSistemas = new SelectList(await _sistemaAppService.AllAsync(), "Id", "Nome");
                    TarefaView.SelectProjetos = new SelectList(await _projetoAppService.AllAsync(), "Id", "Nome");
                    TarefaView.SelectWorkflows = new SelectList(await _workflowAppService.AllAsync(), "Id", "Nome");
                    TarefaView.SelectTipoTarefas = new SelectList(await _tipoTarefaAppService.AllAsync(), "Id", "Nome");
                    
                    TarefaView.User = HttpContext.User;  

                    return View(TarefaView);
                }

                await _tarefaAppService.AddAsync(obj.Tarefa);
                await _tarefaAppService.SaveChangesAsync();

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
                TarefaView.Tarefa  = await _tarefaAppService.GetAsync(id);
                
                TarefaView.SelectSistemas = new SelectList(await _sistemaAppService.AllAsync(), "Id", "Nome");
                TarefaView.SelectProjetos = new SelectList(await _projetoAppService.AllAsync(), "Id", "Nome");
                TarefaView.SelectWorkflows = new SelectList(await _workflowAppService.AllAsync(), "Id", "Nome");
                TarefaView.SelectTipoTarefas = new SelectList(await _tipoTarefaAppService.AllAsync(), "Id", "Nome");
                
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
                    TarefaView.Tarefa  = await _tarefaAppService.GetAsync(obj.Tarefa.Id);
                    
                    TarefaView.SelectSistemas = new SelectList(await _sistemaAppService.AllAsync(), "Id", "Nome");
                    TarefaView.SelectProjetos = new SelectList(await _projetoAppService.AllAsync(), "Id", "Nome");
                    TarefaView.SelectWorkflows = new SelectList(await _workflowAppService.AllAsync(), "Id", "Nome");
                    TarefaView.SelectTipoTarefas = new SelectList(await _tipoTarefaAppService.AllAsync(), "Id", "Nome");

                    return View(TarefaView);
                }

                _tarefaAppService.Update(obj.Tarefa);
                await _tarefaAppService.SaveChangesAsync();

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
                TarefaView.Tarefa  = await _tarefaAppService.GetAsync(id);

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
                    TarefaView.Tarefa  = await _tarefaAppService.GetAsync(obj.Tarefa.Id);

                    return View(TarefaView);
                }

                await _tarefaAppService.RemoveAsync(obj.Tarefa.Id);
                await _tarefaAppService.SaveChangesAsync();

                return RedirectToAction("Listar");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }
    }
}
