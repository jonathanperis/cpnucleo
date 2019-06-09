using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Apontamento;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Apontamento
{
    public class RemoverTest
    {
        private readonly Mock<IApontamentoRepository> _apontamentoRepository;

        public RemoverTest() => _apontamentoRepository = new Mock<IApontamentoRepository>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idApontamento)
        {
            // Arrange
            var apontamentoMock = new ApontamentoItem { };

            _apontamentoRepository.Setup(x => x.ConsultarAsync(idApontamento)).ReturnsAsync(apontamentoMock);

            var RemoverModel = new RemoverModel(_apontamentoRepository.Object);

            // Act
            var actionResult = await RemoverModel.OnGetAsync();

            // Assert
            Assert.NotNull(actionResult);
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnPostAsync(int idApontamento)
        {
            // Arrange
            var apontamentoMock = new ApontamentoItem { IdApontamento = idApontamento };

            _apontamentoRepository.Setup(x => x.RemoverAsync(apontamentoMock));

            var removerModel = new RemoverModel(_apontamentoRepository.Object);

            // Act
            var actionResult = await removerModel.OnPostAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
