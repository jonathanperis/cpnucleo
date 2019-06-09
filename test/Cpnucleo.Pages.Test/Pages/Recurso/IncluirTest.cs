using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Recurso;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Recurso
{
    public class IncluirTest
    {
        private readonly Mock<IRecursoRepository> _recursoRepository;

        public IncluirTest() => _recursoRepository = new Mock<IRecursoRepository>();

        [Theory]
        [InlineData("Recurso de Teste", "recurso.teste", "12345678", "12345678", true)]
        public async Task Test_OnPostAsync(string nome, string login, string senha, string confirmarSenha, bool ativo)
        {
            // Arrange
            RecursoItem recursoMock = new RecursoItem { Nome = nome, Login = login, Senha = senha, ConfirmarSenha = confirmarSenha, Ativo = ativo };

            _recursoRepository.Setup(x => x.IncluirAsync(recursoMock));
            var incluirModel = new IncluirModel(_recursoRepository.Object);

            // Act
            var actionResult = await incluirModel.OnPostAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
