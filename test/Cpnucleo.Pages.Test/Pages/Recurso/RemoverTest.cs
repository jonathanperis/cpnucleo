using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Recurso;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Recurso
{
    public class RemoverTest
    {
        private readonly Mock<IRecursoRepository> _recursoRepository;

        public RemoverTest() => _recursoRepository = new Mock<IRecursoRepository>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idRecurso)
        {
            // Arrange
            var recursoMock = new RecursoItem { };

            _recursoRepository.Setup(x => x.ConsultarAsync(idRecurso)).ReturnsAsync(recursoMock);

            var pageModel = new RemoverModel(_recursoRepository.Object);

            // Act
            var result = await pageModel.OnGetAsync(idRecurso);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(1, "Recurso de Teste", "recurso.teste", "12345678", "12345678", true)]
        public void Test_OnPostAsync(int idRecurso, string nome, string login, string senha, string confirmarSenha, bool ativo)
        {
            // Arrange
            var recursoMock = new RecursoItem { IdRecurso = idRecurso, Nome = nome, Login = login, Senha = senha, ConfirmarSenha = confirmarSenha, Ativo = ativo };

            _recursoRepository.Setup(x => x.RemoverAsync(recursoMock));

            var pageModel = new RemoverModel(_recursoRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

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
