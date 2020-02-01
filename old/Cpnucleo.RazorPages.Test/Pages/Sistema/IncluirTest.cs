using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages.Sistema;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Sistema
{
    public class IncluirTest
    {
        private readonly Mock<ISistemaAppService> _sistemaAppService;

        public IncluirTest()
        {
            _sistemaAppService = new Mock<ISistemaAppService>();
        }

        [Theory]
        [InlineData("Sistema de Teste", "Descrição de Teste")]
        public void Test_OnPost(string nome, string descricao)
        {
            // Arrange
            SistemaViewModel sistemaMock = new SistemaViewModel { Nome = nome, Descricao = descricao };

            IncluirModel pageModel = new IncluirModel(_sistemaAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _sistemaAppService.Setup(x => x.Incluir(sistemaMock));

            PageModelTester<IncluirModel> pageTester = new PageModelTester<IncluirModel>(pageModel);

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