using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages.Impedimento;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Impedimento
{
    public class IncluirTest
    {
        private readonly Mock<ICrudAppService<ImpedimentoViewModel>> _impedimentoAppService;

        public IncluirTest()
        {
            _impedimentoAppService = new Mock<ICrudAppService<ImpedimentoViewModel>>();
        }

        [Theory]
        [InlineData("Impedimento de Teste")]
        public void Test_OnPost(string nome)
        {
            // Arrange
            ImpedimentoViewModel impedimentoMock = new ImpedimentoViewModel { Nome = nome };

            _impedimentoAppService.Setup(x => x.Incluir(impedimentoMock));

            IncluirModel pageModel = new IncluirModel(_impedimentoAppService.Object);
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
            Validation.For(impedimentoMock).ShouldReturn.NoErrors();
        }
    }
}
