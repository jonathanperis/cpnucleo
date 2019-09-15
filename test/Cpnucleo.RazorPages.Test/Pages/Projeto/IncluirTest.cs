using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Projeto;
using Cpnucleo.Application.Interfaces;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System.Collections.Generic;
using Xunit;
using System;

namespace Cpnucleo.RazorPages.Test.Pages.Projeto
{
    public class IncluirTest
    {
        private readonly Mock<IAppService<ProjetoViewModel>> _projetoAppService;
        private readonly Mock<IAppService<SistemaViewModel>> _sistemaAppService;

        public IncluirTest()
        {
            _projetoAppService = new Mock<IAppService<ProjetoViewModel>>();
            _sistemaAppService = new Mock<IAppService<SistemaViewModel>>();
        }

        [Theory]
        [InlineData("Projeto de Teste", 1)]
        public void Test_OnPost(string nome, Guid idSistema)
        {
            // Arrange
            var projetoMock = new ProjetoViewModel { Nome = nome, IdSistema = idSistema };
            var listaMock = new List<SistemaViewModel> { };

            _projetoAppService.Setup(x => x.Incluir(projetoMock));
            _sistemaAppService.Setup(x => x.Listar()).Returns(listaMock);

            var pageModel = new IncluirModel(_projetoAppService.Object, _sistemaAppService.Object);

            var pageTester = new PageModelTester<IncluirModel>(pageModel);

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
            Validation.For(projetoMock).ShouldReturn.NoErrors();
        }
    }
}
