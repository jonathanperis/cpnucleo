using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Impedimento;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Impedimento
{
    public class IncluirTest
    {
        private readonly Mock<IRepository<ImpedimentoItem>> _impedimentoRepository;

        public IncluirTest() => _impedimentoRepository = new Mock<IRepository<ImpedimentoItem>>();

        [Theory]
        [InlineData("Impedimento de Teste")]
        public async Task Test_OnPostAsync(string nome)
        {
            // Arrange
            var impedimentoMock = new ImpedimentoItem { Nome = nome };

            _impedimentoRepository.Setup(x => x.IncluirAsync(impedimentoMock));

            var incluirModel = new IncluirModel(_impedimentoRepository.Object);

            // Act
            var actionResult = await incluirModel.OnPostAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
