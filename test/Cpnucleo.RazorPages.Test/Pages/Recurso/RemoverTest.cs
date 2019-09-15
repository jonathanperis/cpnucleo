using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Recurso;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Recurso
{
    public class RemoverTest
    {
        private readonly Mock<IRecursoAppService> _recursoAppService;

        public RemoverTest() => _recursoAppService = new Mock<IRecursoAppService>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            var recursoMock = new RecursoViewModel { };

            _recursoAppService.Setup(x => x.Consultar(id)).Returns(recursoMock);

            var pageModel = new RemoverModel(_recursoAppService.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Fact]
        public void Test_OnPost()
        {
            // Arrange
            var recursoMock = new RecursoViewModel { };

            _recursoAppService.Setup(x => x.Remover(recursoMock));

            var pageModel = new RemoverModel(_recursoAppService.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
