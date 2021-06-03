using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.Auth;
using Cpnucleo.MVC.Interfaces;
using Cpnucleo.MVC.Models;
using Cpnucleo.MVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cpnucleo.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRecursoService _recursoService;
        private readonly IConfiguration _configuration;

        private HomeView _viewModel;

        public HomeController(IRecursoService recursoService, IConfiguration configuration)
        {
            _recursoService = recursoService;
            _configuration = configuration;
        }

        public HomeView ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    _viewModel = new HomeView();
                }

                return _viewModel;
            }
            set => _viewModel = value;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null, bool logout = false)
        {
            try
            {
                if (logout)
                {
                    await HttpContext.SignOutAsync();

                    return RedirectToAction("Login");
                }

                ViewData["ReturnUrl"] = returnUrl;

                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(HomeView obj, string returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                AuthResponse response = await _recursoService.AuthAsync(new AuthQuery { Login = obj.Usuario, Senha = obj.Senha });

                if (response.Status == OperationResult.Failed)
                {
                    ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                    return View();
                }
                else
                {
                    IEnumerable<Claim> claims = new[]
                    {
                        new Claim(ClaimTypes.PrimarySid, response.Recurso.Id.ToString()),
                        new Claim(ClaimTypes.Hash, response.Recurso.Token)
                    };

                    ClaimsPrincipal principal = ClaimsService.CreateClaimsPrincipal(claims);

                    int.TryParse(_configuration["Cookie:Expires"], out int expiresUtc);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        principal,
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(expiresUtc)
                        });

                    return RedirectToLocal(returnUrl);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        public IActionResult Erro()
        {
            return View();
        }

        public IActionResult Negado()
        {
            return View();
        }

        public IActionResult NaoEncontrado()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Listar", "Apontamento");
            }
        }
    }
}
