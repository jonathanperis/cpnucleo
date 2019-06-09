using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Projeto;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Projeto
{
    public class ListarTest
    {
        private readonly Mock<IRepository<ProjetoItem>> _projetoRepository;

        public ListarTest() => _projetoRepository = new Mock<IRepository<ProjetoItem>>();

        [Fact]
        public async Task Test_OnGetAsync()
        {
            // Arrange
            var listaMock = new List<ProjetoItem> { };

            _projetoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);
            var listarModel = new ListarModel(_projetoRepository.Object);

            // Act
            var actionResult = await listarModel.OnGetAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
