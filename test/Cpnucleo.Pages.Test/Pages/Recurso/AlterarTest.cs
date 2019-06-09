using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Recurso;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Recurso
{
    public class AlterarTest
    {
        private readonly Mock<IRecursoRepository> _recursoRepository;

        public AlterarTest() => _recursoRepository = new Mock<IRecursoRepository>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idRecurso)
        {
            // Arrange
            var recursoMock = new RecursoItem { };

            _recursoRepository.Setup(x => x.ConsultarAsync(idRecurso)).ReturnsAsync(recursoMock);

            var AlterarModel = new AlterarModel(_recursoRepository.Object);

            // Act
            var actionResult = await AlterarModel.OnGetAsync();

            // Assert
            Assert.NotNull(actionResult);
        }

        [Theory]
        [InlineData(1, "Recurso de Teste", "recurso.teste", "12345678", "12345678", true)]
        public async Task Test_OnPostAsync(int idRecurso, string nome, string login, string senha, string confirmarSenha, bool ativo)
        {
            // Arrange
            var recursoMock = new RecursoItem { IdRecurso = idRecurso, Nome = nome, Login = login, Senha = senha, ConfirmarSenha = confirmarSenha, Ativo = ativo};

            _recursoRepository.Setup(x => x.AlterarAsync(recursoMock));

            var alterarModel = new AlterarModel(_recursoRepository.Object);

            // Act
            var actionResult = await alterarModel.OnPostAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
