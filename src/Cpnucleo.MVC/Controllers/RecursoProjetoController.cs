using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoProjeto;
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
    public class RecursoProjetoController : BaseController
    {
        private readonly IRecursoProjetoService _recursoProjetoService;
        private readonly IRecursoService _recursoService;
        private readonly IProjetoService _projetoService;

        private RecursoProjetoView _recursoProjetoView;

        public RecursoProjetoController(IRecursoProjetoService recursoProjetoService,
                                        IRecursoService recursoService,
                                        IProjetoService projetoService)
        {
            _recursoProjetoService = recursoProjetoService;
            _recursoService = recursoService;
            _projetoService = projetoService;
        }

        public RecursoProjetoView RecursoProjetoView
        {
            get
            {
                if (_recursoProjetoView == null)
                {
                    _recursoProjetoView = new RecursoProjetoView();
                }

                return _recursoProjetoView;
            }
            set => _recursoProjetoView = value;
        }

        [HttpGet]
        public async Task<IActionResult> Listar(Guid idProjeto)
        {
            try
            {
                GetByProjetoResponse response = await _recursoProjetoService.GetByProjetoAsync(Token, new GetByProjetoQuery { IdProjeto = idProjeto });
                RecursoProjetoView.Lista = response.RecursoProjetos;

                ViewData["idProjeto"] = idProjeto;

                return View(RecursoProjetoView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Incluir(Guid idProjeto)
        {
            await ObterProjeto(idProjeto);
            await CarregarSelectRecursos();

            return View(RecursoProjetoView);
        }

        [HttpPost]
        public async Task<IActionResult> Incluir(RecursoProjetoView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await ObterProjeto(obj.Projeto.Id);
                    await CarregarSelectRecursos();

                    return View(RecursoProjetoView);
                }

                await _recursoProjetoService.AddAsync(Token, new CreateRecursoProjetoCommand { RecursoProjeto = obj.RecursoProjeto });

                return RedirectToAction("Listar", new { idProjeto = obj.RecursoProjeto.IdProjeto });
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
                GetRecursoProjetoResponse response = await _recursoProjetoService.GetAsync(Token, new GetRecursoProjetoQuery { Id = id });
                RecursoProjetoView.RecursoProjeto = response.RecursoProjeto;

                await CarregarSelectRecursos();

                return View(RecursoProjetoView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(RecursoProjetoView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    GetRecursoProjetoResponse response = await _recursoProjetoService.GetAsync(Token, new GetRecursoProjetoQuery { Id = obj.RecursoProjeto.Id });
                    RecursoProjetoView.RecursoProjeto = response.RecursoProjeto;

                    await CarregarSelectRecursos();

                    return View(RecursoProjetoView);
                }

                await _recursoProjetoService.UpdateAsync(Token, new UpdateRecursoProjetoCommand { RecursoProjeto = obj.RecursoProjeto });

                return RedirectToAction("Listar", new { idProjeto = obj.RecursoProjeto.IdProjeto });
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
                GetRecursoProjetoResponse response = await _recursoProjetoService.GetAsync(Token, new GetRecursoProjetoQuery { Id = id });
                RecursoProjetoView.RecursoProjeto = response.RecursoProjeto;

                return View(RecursoProjetoView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remover(RecursoProjetoView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    GetRecursoProjetoResponse response = await _recursoProjetoService.GetAsync(Token, new GetRecursoProjetoQuery { Id = obj.RecursoProjeto.Id });
                    RecursoProjetoView.RecursoProjeto = response.RecursoProjeto;

                    return View(RecursoProjetoView);
                }

                await _recursoProjetoService.RemoveAsync(Token, new RemoveRecursoProjetoCommand { Id = obj.RecursoProjeto.Id });

                return RedirectToAction("Listar", new { idProjeto = obj.RecursoProjeto.IdProjeto });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        private async Task ObterProjeto(Guid idProjeto)
        {
            Infra.CrossCutting.Util.Queries.Responses.Projeto.GetProjetoResponse response = await _projetoService.GetAsync(Token, new Infra.CrossCutting.Util.Queries.Requests.Projeto.GetProjetoQuery { Id = idProjeto });
            RecursoProjetoView.Projeto = response.Projeto;
        }

        private async Task CarregarSelectRecursos()
        {
            Infra.CrossCutting.Util.Queries.Responses.Recurso.ListRecursoResponse response = await _recursoService.AllAsync(Token, new Infra.CrossCutting.Util.Queries.Requests.Recurso.ListRecursoQuery { });
            RecursoProjetoView.SelectRecursos = new SelectList(response.Recursos, "Id", "Nome");
        }
    }
}
