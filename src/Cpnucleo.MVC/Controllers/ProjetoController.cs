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
    public class ProjetoController : Controller
    {
        private readonly IProjetoAppService _projetoAppService;
        private readonly ISistemaAppService _sistemaAppService;

        private ProjetoView _projetoView;

        public ProjetoController(IProjetoAppService projetoAppService, 
                                 ISistemaAppService sistemaAppService)
        {
            _projetoAppService = projetoAppService;
            _sistemaAppService = sistemaAppService;
        }

        public ProjetoView ProjetoView
        {
            get
            {
                if (_projetoView == null)
                    _projetoView = new ProjetoView();

                return _projetoView;
            }
            set
            {
                _projetoView = value;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                ProjetoView.Lista = await _projetoAppService.AllAsync(true);

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
            ProjetoView.SelectSistemas = new SelectList(await _sistemaAppService.AllAsync(), "Id", "Nome");

            return View(ProjetoView);
        }

        [HttpPost]
        public async Task<IActionResult> Incluir(ProjetoView obj)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ProjetoView.SelectSistemas = new SelectList(await _sistemaAppService.AllAsync(), "Id", "Nome");

                    return View(ProjetoView);
                }

                await _projetoAppService.AddAsync(obj.Projeto);
                await _projetoAppService.SaveChangesAsync();

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
                ProjetoView.Projeto  = await _projetoAppService.GetAsync(id);
                ProjetoView.SelectSistemas = new SelectList(await _sistemaAppService.AllAsync(), "Id", "Nome");

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
                    ProjetoView.Projeto  = await _projetoAppService.GetAsync(obj.Projeto.Id);
                    ProjetoView.SelectSistemas = new SelectList(await _sistemaAppService.AllAsync(), "Id", "Nome");

                    return View(ProjetoView);
                }

                _projetoAppService.Update(obj.Projeto);
                await _projetoAppService.SaveChangesAsync();

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
                ProjetoView.Projeto  = await _projetoAppService.GetAsync(id);

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
                    ProjetoView.Projeto  = await _projetoAppService.GetAsync(obj.Projeto.Id);

                    return View(ProjetoView);
                }

                await _projetoAppService.RemoveAsync(obj.Projeto.Id);
                await _projetoAppService.SaveChangesAsync();

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
