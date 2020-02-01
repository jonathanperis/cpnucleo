using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages.Projeto;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Projeto
{
    public class RemoverTest
    {
        private readonly Mock<IProjetoAppService> _projetoAppService;

        public RemoverTest()
        {
            _projetoAppService = new Mock<IProjetoAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            ProjetoViewModel projetoMock = new ProjetoViewModel { };

            RemoverModel pageModel = new RemoverModel(_projetoAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _projetoAppService.Setup(x => x.Consultar(id)).Returns(projetoMock);

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

            ProjetoViewModel projetoMock = new ProjetoViewModel { };

            RemoverModel pageModel = new RemoverModel(_projetoAppService.Object)
            {
                Projeto = new ProjetoViewModel { Id = id },
                PageContext = PageContextManager.CreatePageContext()
            };

            _projetoAppService.Setup(x => x.Remover(id));

            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
