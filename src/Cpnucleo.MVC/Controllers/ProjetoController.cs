using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Sistema;
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
    public class ProjetoController : BaseController
    {
        private readonly IProjetoService _projetoService;
        private readonly ISistemaService _sistemaService;

        private ProjetoView _projetoView;

        public ProjetoController(IProjetoService projetoService, ISistemaService sistemaService)
        {
            _projetoService = projetoService;
            _sistemaService = sistemaService;
        }

        public ProjetoView ProjetoView
        {
            get
            {
                if (_projetoView == null)
                {
                    _projetoView = new ProjetoView();
                }

                return _projetoView;
            }
            set => _projetoView = value;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                ListProjetoResponse response = await _projetoService.AllAsync(Token, new ListProjetoQuery { GetDependencies = true });
                ProjetoView.Lista = response.Projetos;

                return View(ProjetoView);
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

            return View(ProjetoView);
        }

        [HttpPost]
        public async Task<IActionResult> Incluir(ProjetoView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await CarregarSelectSistemas();

                    return View(ProjetoView);
                }

                await _projetoService.AddAsync(Token, new CreateProjetoCommand { Projeto = obj.Projeto });

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
                GetProjetoResponse response = await _projetoService.GetAsync(Token, new GetProjetoQuery { Id = id });
                ProjetoView.Projeto = response.Projeto;

                await CarregarSelectSistemas();

                return View(ProjetoView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(ProjetoView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    GetProjetoResponse response = await _projetoService.GetAsync(Token, new GetProjetoQuery { Id = obj.Projeto.Id });
                    ProjetoView.Projeto = response.Projeto;

                    await CarregarSelectSistemas();

                    return View(ProjetoView);
                }

                await _projetoService.UpdateAsync(Token, new UpdateProjetoCommand { Projeto = obj.Projeto });

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
                GetProjetoResponse response = await _projetoService.GetAsync(Token, new GetProjetoQuery { Id = id });
                ProjetoView.Projeto = response.Projeto;

                return View(ProjetoView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remover(ProjetoView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    GetProjetoResponse response = await _projetoService.GetAsync(Token, new GetProjetoQuery { Id = obj.Projeto.Id });
                    ProjetoView.Projeto = response.Projeto;

                    return View(ProjetoView);
                }

                await _projetoService.RemoveAsync(Token, new RemoveProjetoCommand { Id = obj.Projeto.Id });

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
            ProjetoView.SelectSistemas = new SelectList(response.Sistemas, "Id", "Nome");
        }
    }
}
