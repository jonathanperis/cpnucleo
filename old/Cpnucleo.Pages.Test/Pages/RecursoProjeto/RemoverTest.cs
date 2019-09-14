using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.RecursoProjeto;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.RecursoProjeto
{
    public class RemoverTest
    {
        private readonly Mock<IRecursoProjetoRepository> _recursoProjetoRepository;

        public RemoverTest() => _recursoProjetoRepository = new Mock<IRecursoProjetoRepository>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGetAsync(int idRecursoProjeto)
        {
            // Arrange
            var recursoProjetoMock = new RecursoProjetoModel { };

            _recursoProjetoRepository.Setup(x => x.ConsultarAsync(idRecursoProjeto)).ReturnsAsync(recursoProjetoMock);

            var pageModel = new RemoverModel(_recursoProjetoRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGetAsync(idRecursoProjeto))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnPostAsync(int idProjeto)
        {
            // Arrange
            var recursoProjetoMock = new RecursoProjetoModel { };

            _recursoProjetoRepository.Setup(x => x.RemoverAsync(recursoProjetoMock));

            var pageModel = new RemoverModel(_recursoProjetoRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnPostAsync(idProjeto))

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
