using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Projeto;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Projeto
{
    public class RemoverTest
    {
        private readonly Mock<IAppService<ProjetoViewModel>> _projetoAppService;

        public RemoverTest()
        {
            _projetoAppService = new Mock<IAppService<ProjetoViewModel>>();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            ProjetoViewModel projetoMock = new ProjetoViewModel { };

            _projetoAppService.Setup(x => x.Consultar(id)).Returns(projetoMock);

            RemoverModel pageModel = new RemoverModel(_projetoAppService.Object);
            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnPost(Guid id)
        {
            // Arrange
            ProjetoViewModel projetoMock = new ProjetoViewModel { };

            _projetoAppService.Setup(x => x.Remover(id));

            RemoverModel pageModel = new RemoverModel(_projetoAppService.Object);
            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
