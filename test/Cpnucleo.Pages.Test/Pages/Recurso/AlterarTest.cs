using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Recurso;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

            var pageModel = new AlterarModel(_recursoRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnGetAsync(idRecurso);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(1, "Recurso de Teste", "recurso.teste", "12345678", "12345678", true)]
        public async Task Test_OnPostAsync(int idRecurso, string nome, string login, string senha, string confirmarSenha, bool ativo)
        {
            // Arrange
            var recursoMock = new RecursoItem { IdRecurso = idRecurso, Nome = nome, Login = login, Senha = senha, ConfirmarSenha = confirmarSenha, Ativo = ativo};

            _recursoRepository.Setup(x => x.AlterarAsync(recursoMock));

            var pageModel = new AlterarModel(_recursoRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }
    }
}
