using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Sistema;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Sistema
{
    public class AlterarTest
    {
        private readonly Mock<IRepository<SistemaItem>> _sistemaRepository;

        public AlterarTest() => _sistemaRepository = new Mock<IRepository<SistemaItem>>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idSistema)
        {
            // Arrange
            var sistemaMock = new SistemaItem { };

            _sistemaRepository.Setup(x => x.ConsultarAsync(idSistema)).ReturnsAsync(sistemaMock);

            var AlterarModel = new AlterarModel(_sistemaRepository.Object);

            // Act
            var actionResult = await AlterarModel.OnGetAsync();

            // Assert
            Assert.NotNull(actionResult);
        }

        [Theory]
        [InlineData(1, "Sistema de Teste", "Descrição de Teste")]
        public async Task Test_OnPostAsync(int idSistema, string nome, string descricao)
        {
            // Arrange
            var sistemaMock = new SistemaItem { IdSistema = idSistema, Nome = nome, Descricao = descricao };

            _sistemaRepository.Setup(x => x.AlterarAsync(sistemaMock));

            var alterarModel = new AlterarModel(_sistemaRepository.Object);

            // Act
            var actionResult = await alterarModel.OnPostAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
