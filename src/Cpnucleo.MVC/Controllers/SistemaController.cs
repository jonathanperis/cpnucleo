using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Sistema;
using Cpnucleo.MVC.Models;
using Cpnucleo.RazorPages.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
{
    //[Authorize]
    public class SistemaController : BaseController
    {
        private readonly ISistemaService _sistemaService;

        private SistemaView _sistemaView;

        public SistemaController(ISistemaService sistemaService)
        {
            _sistemaService = sistemaService;
        }

        public SistemaView SistemaView
        {
            get
            {
                if (_sistemaView == null)
                {
                    _sistemaView = new SistemaView();
                }

                return _sistemaView;
            }
            set => _sistemaView = value;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                ListSistemaResponse response = await _sistemaService.AllAsync(Token, new ListSistemaQuery { GetDependencies = true });
                SistemaView.Lista = response.Sistemas;

                return View(SistemaView);
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
        public async Task<IActionResult> Incluir(SistemaView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                await _sistemaService.AddAsync(Token, new CreateSistemaCommand { Sistema = obj.Sistema });

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
                GetSistemaResponse response = await _sistemaService.GetAsync(Token, new GetSistemaQuery { Id = id });
                SistemaView.Sistema = response.Sistema;

                return View(SistemaView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(SistemaView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    GetSistemaResponse response = await _sistemaService.GetAsync(Token, new GetSistemaQuery { Id = obj.Sistema.Id });
                    SistemaView.Sistema = response.Sistema;

                    return View(SistemaView);
                }

                await _sistemaService.UpdateAsync(Token, new UpdateSistemaCommand { Sistema = obj.Sistema });

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
                GetSistemaResponse response = await _sistemaService.GetAsync(Token, new GetSistemaQuery { Id = id });
                SistemaView.Sistema = response.Sistema;

                return View(SistemaView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remover(SistemaView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    GetSistemaResponse response = await _sistemaService.GetAsync(Token, new GetSistemaQuery { Id = obj.Sistema.Id });
                    SistemaView.Sistema = response.Sistema;

                    return View(SistemaView);
                }

                await _sistemaService.RemoveAsync(Token, new RemoveSistemaCommand { Id = obj.Sistema.Id });

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
