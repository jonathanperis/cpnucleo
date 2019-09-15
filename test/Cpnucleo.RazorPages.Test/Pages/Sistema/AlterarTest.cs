using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Sistema;
using Cpnucleo.Application.Interfaces;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;
using System;

namespace Cpnucleo.RazorPages.Test.Pages.Sistema
{
    public class AlterarTest
    {
        private readonly Mock<IAppService<SistemaViewModel>> _sistemaAppService;

        public AlterarTest() => _sistemaAppService = new Mock<IAppService<SistemaViewModel>>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            var sistemaMock = new SistemaViewModel { };

            _sistemaAppService.Setup(x => x.Consultar(id)).Returns(sistemaMock);

            var pageModel = new AlterarModel(_sistemaAppService.Object);

            var pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1, "Sistema de Teste", "Descrição de Teste")]
        public void Test_OnPost(Guid id, string nome, string descricao)
        {
            // Arrange
            var sistemaMock = new SistemaViewModel { Id = id, Nome = nome, Descricao = descricao };

            _sistemaAppService.Setup(x => x.Alterar(sistemaMock));

            var pageModel = new AlterarModel(_sistemaAppService.Object);

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
            Validation.For(sistemaMock).ShouldReturn.NoErrors();
        }
    }
}
