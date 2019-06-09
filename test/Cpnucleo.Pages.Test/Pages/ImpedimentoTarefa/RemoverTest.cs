using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.ImpedimentoTarefa;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.ImpedimentoTarefa
{
    public class RemoverTest
    {
        private readonly Mock<IImpedimentoTarefaRepository> _impedimentoTarefaRepository;

        public RemoverTest() => _impedimentoTarefaRepository = new Mock<IImpedimentoTarefaRepository>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idImpedimentoTarefa)
        {
            // Arrange
            var impedimentoTarefaMock = new ImpedimentoTarefaItem { };

            _impedimentoTarefaRepository.Setup(x => x.ConsultarAsync(idImpedimentoTarefa)).ReturnsAsync(impedimentoTarefaMock);

            var RemoverModel = new RemoverModel(_impedimentoTarefaRepository.Object);

            // Act
            var actionResult = await RemoverModel.OnGetAsync();

            // Assert
            Assert.NotNull(actionResult);
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnPostAsync(int idImpedimentoTarefa)
        {
            // Arrange
            var impedimentoTarefaMock = new ImpedimentoTarefaItem { IdImpedimentoTarefa = idImpedimentoTarefa };

            _impedimentoTarefaRepository.Setup(x => x.RemoverAsync(impedimentoTarefaMock));

            var removerModel = new RemoverModel(_impedimentoTarefaRepository.Object);

            // Act
            var actionResult = await removerModel.OnPostAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
