using Cpnucleo.Application.Interfaces;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
{
    [Authorize]
    public class SistemaController : Controller
    {
        private readonly ISistemaAppService _sistemaAppService;

        private SistemaView _sistemaView;

        public SistemaController(ISistemaAppService sistemaAppService)
        {
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
