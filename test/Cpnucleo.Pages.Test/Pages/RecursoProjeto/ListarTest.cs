using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.RecursoProjeto;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.RecursoProjeto
{
    public class ListarTest
    {
        private readonly Mock<IRecursoProjetoRepository> _recursoProjetoRepository;

        public ListarTest() => _recursoProjetoRepository = new Mock<IRecursoProjetoRepository>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idProjeto)
        {
            // Arrange
            var listaMock = new List<RecursoProjetoItem> { };

            _recursoProjetoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var listarModel = new ListarModel(_recursoProjetoRepository.Object);

            // Act
            var actionResult = await listarModel.OnGetAsync(idProjeto);

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
