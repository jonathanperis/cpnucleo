using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
{
    [Authorize]
    public class RecursoController : Controller
    {
        private readonly IRecursoAppService _recursoAppService;
        private readonly ICryptographyManager _cryptographyManager;

        private RecursoView _recursoView;

        public RecursoController(IRecursoAppService recursoAppService,
                                 ICryptographyManager cryptographyManager)
        {
            _recursoAppService = recursoAppService;
            _cryptographyManager = cryptographyManager;
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
                RecursoView.Lista = await _recursoAppService.AllAsync();

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

                _cryptographyManager.CryptPbkdf2(obj.Recurso.Senha, out string senhaCrypt, out string salt);

                obj.Recurso.Senha = senhaCrypt;
                obj.Recurso.Salt = salt;

                await _recursoAppService.AddAsync(obj.Recurso);
                await _recursoAppService.SaveChangesAsync();

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
                RecursoView.Recurso = await _recursoAppService.GetAsync(id);

                RecursoView.Recurso.Senha = null;
                RecursoView.Recurso.Salt = null;

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
                    RecursoView.Recurso = await _recursoAppService.GetAsync(obj.Recurso.Id);

                    RecursoView.Recurso.Senha = null;
                    RecursoView.Recurso.Salt = null;

                    return View(RecursoView);
                }

                _cryptographyManager.CryptPbkdf2(obj.Recurso.Senha, out string senhaCrypt, out string salt);

                obj.Recurso.Senha = senhaCrypt;
                obj.Recurso.Salt = salt;

                _recursoAppService.Update(obj.Recurso);
                await _recursoAppService.SaveChangesAsync();

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
                RecursoView.Recurso = await _recursoAppService.GetAsync(id);

                RecursoView.Recurso.Senha = null;
                RecursoView.Recurso.Salt = null;

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
                    RecursoView.Recurso = await _recursoAppService.GetAsync(obj.Recurso.Id);

                    RecursoView.Recurso.Senha = null;
                    RecursoView.Recurso.Salt = null;

                    return View(RecursoView);
                }

                await _recursoAppService.RemoveAsync(obj.Recurso.Id);
                await _recursoAppService.SaveChangesAsync();

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
