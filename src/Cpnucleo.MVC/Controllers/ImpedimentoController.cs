using System;
using Microsoft.AspNetCore.Mvc;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Cpnucleo.Application.Interfaces;

namespace Cpnucleo.MVC.Controllers
{
    [Authorize]
    public class ImpedimentoController : Controller
    {
        private readonly IImpedimentoAppService _impedimentoAppService;

        private ImpedimentoView _impedimentoView;

        public ImpedimentoController(IImpedimentoAppService impedimentoAppService)
        {
            _impedimentoAppService = impedimentoAppService;
        }

        public ImpedimentoView ImpedimentoView
        {
            get
            {
                if (_impedimentoView == null)
                    _impedimentoView = new ImpedimentoView();

                return _impedimentoView;
            }
            set
            {
                _impedimentoView = value;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                ImpedimentoView.Lista = await _impedimentoAppService.AllAsync();

                return View(ImpedimentoView);
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
        public async Task<IActionResult> Incluir(ImpedimentoView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                await _impedimentoAppService.AddAsync(obj.Impedimento);
                await _impedimentoAppService.SaveChangesAsync();

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
                ImpedimentoView.Impedimento  = await _impedimentoAppService.GetAsync(id);

                return View(ImpedimentoView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(ImpedimentoView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ImpedimentoView.Impedimento  = await _impedimentoAppService.GetAsync(obj.Impedimento.Id);

                    return View(ImpedimentoView);
                }

                _impedimentoAppService.Update(obj.Impedimento);
                await _impedimentoAppService.SaveChangesAsync();

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
                ImpedimentoView.Impedimento  = await _impedimentoAppService.GetAsync(id);

                return View(ImpedimentoView);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Remover(ImpedimentoView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ImpedimentoView.Impedimento  = await _impedimentoAppService.GetAsync(obj.Impedimento.Id);

                    return View(ImpedimentoView);
                }

                await _impedimentoAppService.RemoveAsync(obj.Impedimento.Id);
                await _impedimentoAppService.SaveChangesAsync();

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
