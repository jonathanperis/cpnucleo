﻿using System;
using Cpnucleo.Domain.UoW;
using Microsoft.AspNetCore.Mvc;
using Cpnucleo.MVC.Models;
using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;
using Cpnucleo.Domain.Entities;
using Cpnucleo.MVC.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Cpnucleo.MVC.Controllers.V2
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICryptographyManager _cryptographyManager;
        private readonly IConfiguration _configuration;

        public HomeController(IUnitOfWork unitOfWork, 
                                ICryptographyManager cryptographyManager,
                                IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _cryptographyManager = cryptographyManager;
            _configuration = configuration;
        }

        private HomeViewModel _viewModel;

        public HomeViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                    _viewModel = new HomeViewModel();

                return _viewModel;
            }
            set
            {
                _viewModel = value;
            }
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
        public async Task<IActionResult> Login(HomeViewModel obj, string returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                bool valido = false;

                Recurso recurso = _unitOfWork.RecursoRepository.GetByLogin(obj.Usuario);

                if (recurso == null)
                {
                    ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                    return View();
                }

                valido = _cryptographyManager.VerifyPbkdf2(obj.Senha, recurso.Senha, recurso.Salt);

                if (!valido)
                {
                    ModelState.AddModelError(string.Empty, "Usuário ou senha inválidos.");
                    return View();
                }
                else
                {
                    IEnumerable<Claim> claims = new[]
                    {
                        new Claim(ClaimTypes.PrimarySid, recurso.Id.ToString())
                    };

                    ClaimsPrincipal principal = ClaimsService.CreateClaimsPrincipal(claims);

                    int expiresUtc;
                    int.TryParse(_configuration["Cookie:Expires"], out expiresUtc);

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