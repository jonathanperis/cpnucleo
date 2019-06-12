using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.RecursoProjeto;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.RecursoProjeto
{
    public class RemoverTest
    {
        private readonly Mock<IRecursoProjetoRepository> _recursoProjetoRepository;

        public RemoverTest() => _recursoProjetoRepository = new Mock<IRecursoProjetoRepository>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idRecursoProjeto)
        {
            // Arrange
            var recursoProjetoMock = new RecursoProjetoItem { };

            _recursoProjetoRepository.Setup(x => x.ConsultarAsync(idRecursoProjeto)).ReturnsAsync(recursoProjetoMock);

            var RemoverModel = new RemoverModel(_recursoProjetoRepository.Object);

            // Act
            var actionResult = await RemoverModel.OnGetAsync(idRecursoProjeto);

            // Assert
            Assert.NotNull(actionResult);
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnPostAsync(int idProjeto)
        {
            // Arrange
            var recursoProjetoMock = new RecursoProjetoItem { IdRecursoProjeto = idProjeto };

            _recursoProjetoRepository.Setup(x => x.RemoverAsync(recursoProjetoMock));

            var removerModel = new RemoverModel(_recursoProjetoRepository.Object);

            // Act
            var actionResult = await removerModel.OnPostAsync(idProjeto);

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
