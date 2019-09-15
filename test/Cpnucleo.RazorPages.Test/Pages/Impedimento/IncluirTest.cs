using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Impedimento;
using Cpnucleo.Application.Interfaces;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Impedimento
{
    public class IncluirTest
    {
        private readonly Mock<IAppService<ImpedimentoViewModel>> _impedimentoAppService;

        public IncluirTest() => _impedimentoAppService = new Mock<IAppService<ImpedimentoViewModel>>();

        [Theory]
        [InlineData("Impedimento de Teste")]
        public void Test_OnPost(string nome)
        {
            // Arrange
            var impedimentoMock = new ImpedimentoViewModel { Nome = nome };

            _impedimentoAppService.Setup(x => x.Incluir(impedimentoMock));

            var pageModel = new IncluirModel(_impedimentoAppService.Object);

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
            Validation.For(impedimentoMock).ShouldReturn.NoErrors();
        }
    }
}
