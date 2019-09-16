using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Sistema;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Sistema
{
    public class RemoverTest
    {
        private readonly Mock<IAppService<SistemaViewModel>> _sistemaAppService;

        public RemoverTest()
        {
            _sistemaAppService = new Mock<IAppService<SistemaViewModel>>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = new Guid();
            SistemaViewModel sistemaMock = new SistemaViewModel { };

            _sistemaAppService.Setup(x => x.Consultar(id)).Returns(sistemaMock);

            RemoverModel pageModel = new RemoverModel(_sistemaAppService.Object);
            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

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
            Guid id = new Guid();
            _sistemaAppService.Setup(x => x.Remover(id));

            RemoverModel pageModel = new RemoverModel(_sistemaAppService.Object);
            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
