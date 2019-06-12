using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.ImpedimentoTarefa;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.ImpedimentoTarefa
{
    public class ListarTest
    {
        private readonly Mock<IImpedimentoTarefaRepository> _impedimentoTarefaRepository;

        public ListarTest() => _impedimentoTarefaRepository = new Mock<IImpedimentoTarefaRepository>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idTarefa)
        {
            // Arrange
            var listaMock = new List<ImpedimentoTarefaItem> { };

            _impedimentoTarefaRepository.Setup(x => x.ListarPoridTarefaAsync(idTarefa)).ReturnsAsync(listaMock);

            var listarModel = new ListarModel(_impedimentoTarefaRepository.Object);

            // Act
            var actionResult = await listarModel.OnGetAsync(idTarefa);

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
