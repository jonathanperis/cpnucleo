using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Impedimento;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Impedimento
{
    public class RemoverTest
    {
        private readonly Mock<IRepository<ImpedimentoItem>> _impedimentoRepository;

        public RemoverTest() => _impedimentoRepository = new Mock<IRepository<ImpedimentoItem>>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idImpedimento)
        {
            // Arrange
            var impedimentoMock = new ImpedimentoItem { };

            _impedimentoRepository.Setup(x => x.ConsultarAsync(idImpedimento)).ReturnsAsync(impedimentoMock);
            var RemoverModel = new RemoverModel(_impedimentoRepository.Object);

            // Act
            var actionResult = await RemoverModel.OnGetAsync();

            // Assert
            Assert.NotNull(actionResult);
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnPostAsync(int idImpedimento)
        {
            // Arrange
            ImpedimentoItem impedimentoMock = new ImpedimentoItem { IdImpedimento = idImpedimento };

            _impedimentoRepository.Setup(x => x.RemoverAsync(impedimentoMock));
            var removerModel = new RemoverModel(_impedimentoRepository.Object);

            // Act
            var actionResult = await removerModel.OnPostAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
