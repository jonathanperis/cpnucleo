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
    public class AlterarTest
    {
        private readonly Mock<IAppService<ProjetoViewModel>> _projetoAppService;
        private readonly Mock<IAppService<SistemaViewModel>> _sistemaAppService;

        public AlterarTest()
        {
            _projetoAppService = new Mock<IAppService<ProjetoViewModel>>();
            _sistemaAppService = new Mock<IAppService<SistemaViewModel>>();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            var projetoMock = new ProjetoViewModel { };
            var listaMock = new List<SistemaViewModel> { };

            _projetoAppService.Setup(x => x.Consultar(id)).Returns(projetoMock);
            _sistemaAppService.Setup(x => x.Listar()).Returns(listaMock);

            var pageModel = new AlterarModel(_projetoAppService.Object, _sistemaAppService.Object);

            var pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1, "Projeto de Teste", 1)]
        public void Test_OnPost(Guid id, string nome, Guid idSistema)
        {
            // Arrange
            var projetoMock = new ProjetoViewModel { Id = id, Nome = nome, IdSistema = idSistema };
            var listaMock = new List<SistemaViewModel> { };

            _projetoAppService.Setup(x => x.Alterar(projetoMock));
            _sistemaAppService.Setup(x => x.Listar()).Returns(listaMock);

            var pageModel = new AlterarModel(_projetoAppService.Object, _sistemaAppService.Object);

            var pageTester = new PageModelTester<AlterarModel>(pageModel);

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
