using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Recurso;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Recurso
{
    public class RemoverTest
    {
        private readonly Mock<IRecursoRepository> _recursoRepository;

        public RemoverTest() => _recursoRepository = new Mock<IRecursoRepository>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGetAsync(int idRecurso)
        {
            // Arrange
            var recursoMock = new RecursoModel { };

            _recursoRepository.Setup(x => x.ConsultarAsync(idRecurso)).ReturnsAsync(recursoMock);

            var pageModel = new RemoverModel(_recursoRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGetAsync(idRecurso))

                // Assert
                .TestPage();
        }

        [Fact]
        public void Test_OnPostAsync()
        {
            // Arrange
            var recursoMock = new RecursoModel { };

            _recursoRepository.Setup(x => x.RemoverAsync(recursoMock));

            var pageModel = new RemoverModel(_recursoRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPostAsync)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
