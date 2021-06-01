using Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Impedimento;
using Cpnucleo.MVC.Interfaces;
using Cpnucleo.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
{
    [Authorize]
    public class ImpedimentoController : BaseController
    {
        private readonly IImpedimentoService _impedimentoService;

        private ImpedimentoView _impedimentoView;

        public ImpedimentoController(IImpedimentoService impedimentoService)
        {
            _impedimentoService = impedimentoService;
        }

        public ImpedimentoView ImpedimentoView
        {
            get
            {
                if (_impedimentoView == null)
                {
                    _impedimentoView = new ImpedimentoView();
                }

                return _impedimentoView;
            }
            set => _impedimentoView = value;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            try
            {
                ListImpedimentoResponse response = await _impedimentoService.AllAsync(Token, new ListImpedimentoQuery { });
                ImpedimentoView.Lista = response.Impedimentos;

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

                await _impedimentoService.AddAsync(Token, new CreateImpedimentoCommand { Impedimento = obj.Impedimento });

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
                GetImpedimentoResponse response = await _impedimentoService.GetAsync(Token, new GetImpedimentoQuery { Id = id });
                ImpedimentoView.Impedimento = response.Impedimento;

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
                    GetImpedimentoResponse response = await _impedimentoService.GetAsync(Token, new GetImpedimentoQuery { Id = obj.Impedimento.Id });
                    ImpedimentoView.Impedimento = response.Impedimento;

                    return View(ImpedimentoView);
                }

                await _impedimentoService.UpdateAsync(Token, new UpdateImpedimentoCommand { Impedimento = obj.Impedimento });

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
                GetImpedimentoResponse response = await _impedimentoService.GetAsync(Token, new GetImpedimentoQuery { Id = id });
                ImpedimentoView.Impedimento = response.Impedimento;

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
                    GetImpedimentoResponse response = await _impedimentoService.GetAsync(Token, new GetImpedimentoQuery { Id = obj.Impedimento.Id });
                    ImpedimentoView.Impedimento = response.Impedimento;

                    return View(ImpedimentoView);
                }

                await _impedimentoService.RemoveAsync(Token, new RemoveImpedimentoCommand { Id = obj.Impedimento.Id });

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
