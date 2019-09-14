using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Recurso;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Recurso
{
    public class IncluirTest
    {
        private readonly Mock<IRecursoRepository> _recursoRepository;

        public IncluirTest() => _recursoRepository = new Mock<IRecursoRepository>();

        [Theory]
        [InlineData("Recurso de Teste", "recurso.teste", "12345678", "12345678", true)]
        public void Test_OnPostAsync(string nome, string login, string senha, string confirmarSenha, bool ativo)
        {
            // Arrange
            var recursoMock = new RecursoModel { Nome = nome, Login = login, Senha = senha, ConfirmarSenha = confirmarSenha, Ativo = ativo };

            _recursoRepository.Setup(x => x.IncluirAsync(recursoMock));

            var pageModel = new IncluirModel(_recursoRepository.Object);

            var pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPostAsync)

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => x.OnPostAsync)

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(recursoMock).ShouldReturn.NoErrors();
        }
    }
}
