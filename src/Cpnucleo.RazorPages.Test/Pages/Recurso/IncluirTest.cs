using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages.Recurso;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Recurso
{
    public class IncluirTest
    {
        private readonly Mock<IRecursoAppService> _recursoAppService;

        public IncluirTest()
        {
            _recursoAppService = new Mock<IRecursoAppService>();
        }

        [Theory]
        [InlineData("Recurso de Teste", "recurso.teste", "12345678", "12345678", true)]
        public void Test_OnPost(string nome, string login, string senha, string confirmarSenha, bool ativo)
        {
            // Arrange
            RecursoViewModel recursoMock = new RecursoViewModel { Nome = nome, Login = login, Senha = senha, ConfirmarSenha = confirmarSenha, Ativo = ativo };

            IncluirModel pageModel = new IncluirModel(_recursoAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _recursoAppService.Setup(x => x.Incluir(recursoMock));

            PageModelTester<IncluirModel> pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(recursoMock).ShouldReturn.NoErrors();
        }
    }
}
