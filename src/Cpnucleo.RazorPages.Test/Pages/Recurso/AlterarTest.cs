﻿using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages.Recurso;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Recurso
{
    public class AlterarTest
    {
        private readonly Mock<IRecursoAppService> _recursoAppService;

        public AlterarTest()
        {
            _recursoAppService = new Mock<IRecursoAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            RecursoViewModel recursoMock = new RecursoViewModel { };

            AlterarModel pageModel = new AlterarModel(_recursoAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _recursoAppService.Setup(x => x.Consultar(id)).Returns(recursoMock);

            PageModelTester<AlterarModel> pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData("Recurso de Teste", "recurso.teste", "12345678", "12345678")]
        public void Test_OnPost(string nome, string login, string senha, string confirmarSenha)
        {
            // Arrange
            Guid id = Guid.NewGuid();

            RecursoViewModel recursoMock = new RecursoViewModel { Id = id, Nome = nome, Login = login, Senha = senha, ConfirmarSenha = confirmarSenha };

            AlterarModel pageModel = new AlterarModel(_recursoAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _recursoAppService.Setup(x => x.Alterar(recursoMock));

            PageModelTester<AlterarModel> pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(recursoMock).ShouldReturn.NoErrors();
        }
    }
}