using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Sistema;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Sistema
{
    public class ListarTest
    {
        private readonly Mock<IRepository<SistemaItem>> _sistemaRepository;

        public ListarTest() => _sistemaRepository = new Mock<IRepository<SistemaItem>>();

        [Fact]
        public async Task Test_OnGetAsync()
        {
            // Arrange
            var listaMock = new List<SistemaItem> { };

            _sistemaRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);
            var listarModel = new ListarModel(_sistemaRepository.Object);

            // Act
            var actionResult = await listarModel.OnGetAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
