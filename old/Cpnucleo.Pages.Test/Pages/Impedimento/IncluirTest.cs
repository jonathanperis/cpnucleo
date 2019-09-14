using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Impedimento;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Impedimento
{
    public class IncluirTest
    {
        private readonly Mock<IRepository<ImpedimentoModel>> _impedimentoRepository;

        public IncluirTest() => _impedimentoRepository = new Mock<IRepository<ImpedimentoModel>>();

        [Theory]
        [InlineData("Impedimento de Teste")]
        public void Test_OnPostAsync(string nome)
        {
            // Arrange
            var impedimentoMock = new ImpedimentoModel { Nome = nome };

            _impedimentoRepository.Setup(x => x.IncluirAsync(impedimentoMock));

            var pageModel = new IncluirModel(_impedimentoRepository.Object);

            var pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPostAsync)

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => x.OnPostAsync)

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(impedimentoMock).ShouldReturn.NoErrors();
        }
    }
}
