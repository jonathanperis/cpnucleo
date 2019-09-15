using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Recurso;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Recurso
{
    public class AlterarTest
    {
        private readonly Mock<IRecursoAppService> _recursoAppService;

        public AlterarTest()
        {
            _recursoAppService = new Mock<IRecursoAppService>();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            RecursoViewModel recursoMock = new RecursoViewModel { };

            _recursoAppService.Setup(x => x.Consultar(id)).Returns(recursoMock);

            AlterarModel pageModel = new AlterarModel(_recursoAppService.Object);
            PageModelTester<AlterarModel> pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1, "Recurso de Teste", "recurso.teste", "12345678", "12345678", true)]
        public void Test_OnPost(Guid id, string nome, string login, string senha, string confirmarSenha, bool ativo)
        {
            // Arrange
            RecursoViewModel recursoMock = new RecursoViewModel { Id = id, Nome = nome, Login = login, Senha = senha, ConfirmarSenha = confirmarSenha, Ativo = ativo };

            _recursoAppService.Setup(x => x.Alterar(recursoMock));

            AlterarModel pageModel = new AlterarModel(_recursoAppService.Object);
            PageModelTester<AlterarModel> pageTester = new PageModelTester<AlterarModel>(pageModel);

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
