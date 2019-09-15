using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Projeto;
using Cpnucleo.Application.Interfaces;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using Xunit;
using System;

namespace Cpnucleo.RazorPages.Test.Pages.Projeto
{
    public class RemoverTest
    {
        private readonly Mock<IAppService<ProjetoViewModel>> _projetoAppService;

        public RemoverTest() => _projetoAppService = new Mock<IAppService<ProjetoViewModel>>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            var projetoMock = new ProjetoViewModel { };

            _projetoAppService.Setup(x => x.Consultar(id)).Returns(projetoMock);

            var pageModel = new RemoverModel(_projetoAppService.Object);

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
            var projetoMock = new ProjetoViewModel { };

            _projetoAppService.Setup(x => x.Remover(projetoMock));

            var pageModel = new RemoverModel(_projetoAppService.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
