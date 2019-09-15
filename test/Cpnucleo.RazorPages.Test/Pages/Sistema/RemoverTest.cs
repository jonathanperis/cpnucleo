using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Sistema;
using Cpnucleo.Application.Interfaces;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;
using System;

namespace Cpnucleo.RazorPages.Test.Pages.Sistema
{
    public class RemoverTest
    {
        private readonly Mock<IAppService<SistemaViewModel>> _sistemaAppService;

        public RemoverTest() => _sistemaAppService = new Mock<IAppService<SistemaViewModel>>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            var sistemaMock = new SistemaViewModel { };

            _sistemaAppService.Setup(x => x.Consultar(id)).Returns(sistemaMock);

            var pageModel = new RemoverModel(_sistemaAppService.Object);

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
            var sistemaMock = new SistemaViewModel { };

            _sistemaAppService.Setup(x => x.Remover(sistemaMock));

            var pageModel = new RemoverModel(_sistemaAppService.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
