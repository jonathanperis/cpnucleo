using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Recurso;
using Cpnucleo.MVC.Interfaces;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
{
    [Authorize]
    public class RecursoController : BaseController
    {
        private readonly IRecursoService _recursoService;

        private RecursoView _recursoView;

        public RecursoController(IRecursoService recursoService)
        {
            _recursoService = recursoService;
        }

        public RecursoView RecursoView
        {
            get
            {
                if (_recursoView == null)
                {
                    _recursoView = new RecursoView();
                }

                return _recursoView;
            }
            set => _recursoView = value;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                ListRecursoResponse response = await _recursoService.AllAsync(Token, new ListRecursoQuery { });
                RecursoView.Lista = response.Recursos;

                return View(RecursoView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpGet]
        public IActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Incluir(RecursoView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                await _recursoService.AddAsync(Token, new CreateRecursoCommand { Recurso = obj.Recurso });

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
                GetRecursoResponse response = await _recursoService.GetAsync(Token, new GetRecursoQuery { Id = id });
                RecursoView.Recurso = response.Recurso;

                return View(RecursoView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(RecursoView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    GetRecursoResponse response = await _recursoService.GetAsync(Token, new GetRecursoQuery { Id = obj.Recurso.Id });
                    RecursoView.Recurso = response.Recurso;

                    return View(RecursoView);
                }

                await _recursoService.UpdateAsync(Token, new UpdateRecursoCommand { Recurso = obj.Recurso });

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
                GetRecursoResponse response = await _recursoService.GetAsync(Token, new GetRecursoQuery { Id = id });
                RecursoView.Recurso = response.Recurso;

                return View(RecursoView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remover(RecursoView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    GetRecursoResponse response = await _recursoService.GetAsync(Token, new GetRecursoQuery { Id = obj.Recurso.Id });
                    RecursoView.Recurso = response.Recurso;

                    return View(RecursoView);
                }

                await _recursoService.RemoveAsync(Token, new RemoveRecursoCommand { Id = obj.Recurso.Id });

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
