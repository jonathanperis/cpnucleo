using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Projeto;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Projeto
{
    public class RemoverTest
    {
        private readonly Mock<IRepository<ProjetoModel>> _projetoRepository;

        public RemoverTest() => _projetoRepository = new Mock<IRepository<ProjetoModel>>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGetAsync(int idProjeto)
        {
            // Arrange
            var projetoMock = new ProjetoModel { };

            _projetoRepository.Setup(x => x.ConsultarAsync(idProjeto)).ReturnsAsync(projetoMock);

            var pageModel = new RemoverModel(_projetoRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGetAsync(idProjeto))

                // Assert
                .TestPage();
        }

        [Fact]
        public void Test_OnPostAsync()
        {
            // Arrange
            var projetoMock = new ProjetoModel { };

            _projetoRepository.Setup(x => x.RemoverAsync(projetoMock));

            var pageModel = new RemoverModel(_projetoRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPostAsync)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
