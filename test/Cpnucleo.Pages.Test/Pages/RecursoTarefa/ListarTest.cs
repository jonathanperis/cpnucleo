using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.RecursoTarefa;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.RecursoTarefa
{
    public class ListarTest
    {
        private readonly Mock<IRecursoTarefaRepository> _sistemaRepository;

        public ListarTest() => _sistemaRepository = new Mock<IRecursoTarefaRepository>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idTarefa)
        {
            // Arrange
            var listaMock = new List<RecursoTarefaItem> { };

            _sistemaRepository.Setup(x => x.ListarPoridTarefaAsync(idTarefa)).ReturnsAsync(listaMock);

            var listarModel = new ListarModel(_sistemaRepository.Object);

            // Act
            var actionResult = await listarModel.OnGetAsync(idTarefa);

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
