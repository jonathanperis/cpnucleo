using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.CreateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.RemoveSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.UpdateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.GetSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.ListSistema;
using Cpnucleo.MVC.Models;
using MagicOnion.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
{
    [Authorize]
    public class SistemaController : BaseController
    {
        private readonly ISistemaGrpcService _sistemaGrpcService;

        private SistemaView _sistemaView;

        public SistemaController(IConfiguration configuration)
            : base(configuration)
        {
            _sistemaGrpcService = MagicOnionClient.Create<ISistemaGrpcService>(CreateAuthenticatedChannel());
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
                ListSistemaResponse response = await _sistemaGrpcService.AllAsync(new ListSistemaQuery { });
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

                await _sistemaGrpcService.AddAsync(new CreateSistemaCommand { Sistema = obj.Sistema });

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
                GetSistemaResponse response = await _sistemaGrpcService.GetAsync(new GetSistemaQuery { Id = id });
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
                    GetSistemaResponse response = await _sistemaGrpcService.GetAsync(new GetSistemaQuery { Id = obj.Sistema.Id });
                    SistemaView.Sistema = response.Sistema;

                    return View(SistemaView);
                }

                await _sistemaGrpcService.UpdateAsync(new UpdateSistemaCommand { Sistema = obj.Sistema });

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
                GetSistemaResponse response = await _sistemaGrpcService.GetAsync(new GetSistemaQuery { Id = id });
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
                    GetSistemaResponse response = await _sistemaGrpcService.GetAsync(new GetSistemaQuery { Id = obj.Sistema.Id });
                    SistemaView.Sistema = response.Sistema;

                    return View(SistemaView);
                }

                await _sistemaGrpcService.RemoveAsync(new RemoveSistemaCommand { Id = obj.Sistema.Id });

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
