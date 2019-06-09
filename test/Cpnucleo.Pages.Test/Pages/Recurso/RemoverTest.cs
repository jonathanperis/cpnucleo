using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Recurso;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Recurso
{
    public class RemoverTest
    {
        private readonly Mock<IRecursoRepository> _recursoRepository;

        public RemoverTest() => _recursoRepository = new Mock<IRecursoRepository>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idRecurso)
        {
            // Arrange
            var recursoMock = new RecursoItem { };

            _recursoRepository.Setup(x => x.ConsultarAsync(idRecurso)).ReturnsAsync(recursoMock);
            var RemoverModel = new RemoverModel(_recursoRepository.Object);

            // Act
            var actionResult = await RemoverModel.OnGetAsync();

            // Assert
            Assert.NotNull(actionResult);
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnPostAsync(int idRecurso)
        {
            // Arrange
            RecursoItem recursoMock = new RecursoItem { IdRecurso = idRecurso };

            _recursoRepository.Setup(x => x.RemoverAsync(recursoMock));
            var removerModel = new RemoverModel(_recursoRepository.Object);

            // Act
            var actionResult = await removerModel.OnPostAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
