using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Projeto;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Projeto
{
    public class RemoverTest
    {
        private readonly Mock<IRepository<ProjetoItem>> _projetoRepository;

        public RemoverTest() => _projetoRepository = new Mock<IRepository<ProjetoItem>>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idProjeto)
        {
            // Arrange
            var projetoMock = new ProjetoItem { };

            _projetoRepository.Setup(x => x.ConsultarAsync(idProjeto)).ReturnsAsync(projetoMock);
            var RemoverModel = new RemoverModel(_projetoRepository.Object);

            // Act
            var actionResult = await RemoverModel.OnGetAsync();

            // Assert
            Assert.NotNull(actionResult);
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnPostAsync(int idProjeto)
        {
            // Arrange
            ProjetoItem projetoMock = new ProjetoItem { IdProjeto = idProjeto };

            _projetoRepository.Setup(x => x.RemoverAsync(projetoMock));
            var removerModel = new RemoverModel(_projetoRepository.Object);

            // Act
            var actionResult = await removerModel.OnPostAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
