using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Apontamento;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Apontamento
{
    public class RemoverTest
    {
        private readonly Mock<IApontamentoRepository> _apontamentoRepository;

        public RemoverTest() => _apontamentoRepository = new Mock<IApontamentoRepository>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGetAsync(int idApontamento)
        {
            // Arrange
            var apontamentoMock = new ApontamentoModel { };

            _apontamentoRepository.Setup(x => x.ConsultarAsync(idApontamento)).ReturnsAsync(apontamentoMock);

            var pageModel = new RemoverModel(_apontamentoRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGetAsync(idApontamento))

                // Assert
                .TestPage();
        }

        [Fact]
        public void Test_OnPostAsync()
        {
            // Arrange
            var apontamentoMock = new ApontamentoModel { };

            _apontamentoRepository.Setup(x => x.RemoverAsync(apontamentoMock));

            var pageModel = new RemoverModel(_apontamentoRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPostAsync)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
