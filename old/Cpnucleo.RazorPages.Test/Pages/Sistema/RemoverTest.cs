using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages.Sistema;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Sistema
{
    public class RemoverTest
    {
        private readonly Mock<ISistemaAppService> _sistemaAppService;

        public RemoverTest()
        {
            _sistemaAppService = new Mock<ISistemaAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            SistemaViewModel sistemaMock = new SistemaViewModel { };

            RemoverModel pageModel = new RemoverModel(_sistemaAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _sistemaAppService.Setup(x => x.Consultar(id)).Returns(sistemaMock);

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
            Guid id = Guid.NewGuid();

            RemoverModel pageModel = new RemoverModel(_sistemaAppService.Object)
            {
                Sistema = new SistemaViewModel { Id = id },
                PageContext = PageContextManager.CreatePageContext()
            };

            _sistemaAppService.Setup(x => x.Remover(id));

            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
