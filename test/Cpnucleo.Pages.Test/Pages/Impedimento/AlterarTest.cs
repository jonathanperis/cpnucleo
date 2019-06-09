using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Impedimento;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Impedimento
{
    public class AlterarTest
    {
        private readonly Mock<IRepository<ImpedimentoItem>> _impedimentoRepository;

        public AlterarTest() => _impedimentoRepository = new Mock<IRepository<ImpedimentoItem>>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idImpedimento)
        {
            // Arrange
            var impedimentoMock = new ImpedimentoItem { };

            _impedimentoRepository.Setup(x => x.ConsultarAsync(idImpedimento)).ReturnsAsync(impedimentoMock);
            var AlterarModel = new AlterarModel(_impedimentoRepository.Object);

            // Act
            var actionResult = await AlterarModel.OnGetAsync();

            // Assert
            Assert.NotNull(actionResult);
        }

        [Theory]
        [InlineData(1, "Impedimento de Teste")]
        public async Task Test_OnPostAsync(int idImpedimento, string nome)
        {
            // Arrange
            ImpedimentoItem impedimentoMock = new ImpedimentoItem { IdImpedimento = idImpedimento, Nome = nome };

            _impedimentoRepository.Setup(x => x.AlterarAsync(impedimentoMock));
            var alterarModel = new AlterarModel(_impedimentoRepository.Object);

            // Act
            var actionResult = await alterarModel.OnPostAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
