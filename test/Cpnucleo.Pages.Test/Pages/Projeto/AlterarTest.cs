using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Projeto;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Projeto
{
    public class AlterarTest
    {
        private readonly Mock<IRepository<ProjetoItem>> _projetoRepository;
        private readonly Mock<IRepository<SistemaItem>> _sistemaRepository;

        public AlterarTest()
        {
            _projetoRepository = new Mock<IRepository<ProjetoItem>>();
            _sistemaRepository = new Mock<IRepository<SistemaItem>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idProjeto)
        {
            // Arrange
            var projetoMock = new ProjetoItem { };
            var listaMock = new List<SistemaItem> { };

            _projetoRepository.Setup(x => x.ConsultarAsync(idProjeto)).ReturnsAsync(projetoMock);
            _sistemaRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var AlterarModel = new AlterarModel(_projetoRepository.Object, _sistemaRepository.Object);

            // Act
            var actionResult = await AlterarModel.OnGetAsync();

            // Assert
            Assert.NotNull(actionResult);
        }

        [Theory]
        [InlineData(1, "Projeto de Teste", 1)]
        public async Task Test_OnPostAsync(int idProjeto, string nome, int idSistema)
        {
            // Arrange
            var projetoMock = new ProjetoItem { IdProjeto = idProjeto, Nome = nome, IdSistema = idSistema };
            var listaMock = new List<SistemaItem> { };

            _projetoRepository.Setup(x => x.AlterarAsync(projetoMock));
            _sistemaRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var alterarModel = new AlterarModel(_projetoRepository.Object, _sistemaRepository.Object);

            // Act
            var actionResult = await alterarModel.OnPostAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
