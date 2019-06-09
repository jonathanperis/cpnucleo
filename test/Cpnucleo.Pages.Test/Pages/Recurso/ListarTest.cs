using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Recurso;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Recurso
{
    public class ListarTest
    {
        private readonly Mock<IRecursoRepository> _recursoRepository;

        public ListarTest() => _recursoRepository = new Mock<IRecursoRepository>();

        [Fact]
        public async Task Test_OnGetAsync()
        {
            // Arrange
            var listaMock = new List<RecursoItem> { };

            _recursoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);
            var listarModel = new ListarModel(_recursoRepository.Object);

            // Act
            var actionResult = await listarModel.OnGetAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
