using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages.Sistema;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Sistema
{
    public class AlterarTest
    {
        private readonly Mock<ISistemaAppService> _sistemaAppService;

        public AlterarTest()
        {
            _sistemaAppService = new Mock<ISistemaAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            SistemaViewModel sistemaMock = new SistemaViewModel { };

            AlterarModel pageModel = new AlterarModel(_sistemaAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _sistemaAppService.Setup(x => x.Consultar(id)).Returns(sistemaMock);

            PageModelTester<AlterarModel> pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData("Sistema de Teste", "Descrição de Teste")]
        public void Test_OnPost(string nome, string descricao)
        {
            // Arrange
            Guid id = Guid.NewGuid();

            SistemaViewModel sistemaMock = new SistemaViewModel { Id = id, Nome = nome, Descricao = descricao };

            AlterarModel pageModel = new AlterarModel(_sistemaAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _sistemaAppService.Setup(x => x.Alterar(sistemaMock));

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
            Validation.For(sistemaMock).ShouldReturn.NoErrors();
        }
    }
}
