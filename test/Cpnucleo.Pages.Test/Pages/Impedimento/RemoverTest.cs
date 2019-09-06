using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Impedimento;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Impedimento
{
    public class RemoverTest
    {
        private readonly Mock<IRepository<ImpedimentoModel>> _impedimentoRepository;

        public RemoverTest() => _impedimentoRepository = new Mock<IRepository<ImpedimentoModel>>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGetAsync(int idImpedimento)
        {
            // Arrange
            var impedimentoMock = new ImpedimentoModel { };

            _impedimentoRepository.Setup(x => x.ConsultarAsync(idImpedimento)).ReturnsAsync(impedimentoMock);

            var pageModel = new RemoverModel(_impedimentoRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGetAsync(idImpedimento))

                // Assert
                .TestPage();
        }

        [Fact]
        public void Test_OnPostAsync()
        {
            // Arrange
            var impedimentoMock = new ImpedimentoModel { };

            _impedimentoRepository.Setup(x => x.RemoverAsync(impedimentoMock));

            var pageModel = new RemoverModel(_impedimentoRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPostAsync)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
