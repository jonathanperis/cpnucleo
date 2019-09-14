using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Sistema;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Sistema
{
    public class RemoverTest
    {
        private readonly Mock<IRepository<SistemaModel>> _sistemaRepository;

        public RemoverTest() => _sistemaRepository = new Mock<IRepository<SistemaModel>>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGetAsync(int idSistema)
        {
            // Arrange
            var sistemaMock = new SistemaModel { };

            _sistemaRepository.Setup(x => x.ConsultarAsync(idSistema)).ReturnsAsync(sistemaMock);

            var pageModel = new RemoverModel(_sistemaRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGetAsync(idSistema))

                // Assert
                .TestPage();
        }

        [Fact]
        public void Test_OnPostAsync()
        {
            // Arrange
            var sistemaMock = new SistemaModel { };

            _sistemaRepository.Setup(x => x.RemoverAsync(sistemaMock));

            var pageModel = new RemoverModel(_sistemaRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPostAsync)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
