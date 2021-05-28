using Cpnucleo.Application.Interfaces;
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
        private readonly ISistemaAppService _sistemaAppService;

        private SistemaView _sistemaView;

        public SistemaController(ISistemaService sistemaService, ISistemaAppService sistemaAppService)
        {
            _sistemaService = sistemaService;
            _sistemaAppService = sistemaAppService;
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

                SistemaView.Lista = await _sistemaAppService.AllAsync();

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

                await _sistemaAppService.AddAsync(obj.Sistema);
                await _sistemaAppService.SaveChangesAsync();

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
                SistemaView.Sistema = await _sistemaAppService.GetAsync(id);

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
                    SistemaView.Sistema = await _sistemaAppService.GetAsync(obj.Sistema.Id);

                    return View(SistemaView);
                }

                _sistemaAppService.Update(obj.Sistema);
                await _sistemaAppService.SaveChangesAsync();

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
                SistemaView.Sistema = await _sistemaAppService.GetAsync(id);

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
                    SistemaView.Sistema = await _sistemaAppService.GetAsync(obj.Sistema.Id);

                    return View(SistemaView);
                }

                await _sistemaAppService.RemoveAsync(obj.Sistema.Id);
                await _sistemaAppService.SaveChangesAsync();

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
