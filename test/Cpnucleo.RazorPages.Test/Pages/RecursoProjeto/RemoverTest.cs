using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.RecursoProjeto;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.RecursoProjeto
{
    public class RemoverTest
    {
        private readonly Mock<IRecursoProjetoAppService> _recursoProjetoAppService;

        public RemoverTest() => _recursoProjetoAppService = new Mock<IRecursoProjetoAppService>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid idRecursoProjeto)
        {
            // Arrange
            var recursoProjetoMock = new RecursoProjetoViewModel { };

            _recursoProjetoAppService.Setup(x => x.Consultar(idRecursoProjeto)).Returns(recursoProjetoMock);

            var pageModel = new RemoverModel(_recursoProjetoAppService.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(idRecursoProjeto))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnPost(Guid idProjeto)
        {
            // Arrange
            var recursoProjetoMock = new RecursoProjetoViewModel { };

            _recursoProjetoAppService.Setup(x => x.Remover(recursoProjetoMock));

            var pageModel = new RemoverModel(_recursoProjetoAppService.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnPost())

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
