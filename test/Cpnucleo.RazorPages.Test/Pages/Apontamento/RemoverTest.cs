using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Apontamento;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Apontamento
{
    public class RemoverTest
    {
        private readonly Mock<IApontamentoAppService> _apontamentoAppService;

        public RemoverTest()
        {
            _apontamentoAppService = new Mock<IApontamentoAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = new Guid();
            ApontamentoViewModel apontamentoMock = new ApontamentoViewModel { };

            _apontamentoAppService.Setup(x => x.Consultar(id)).Returns(apontamentoMock);

            RemoverModel pageModel = new RemoverModel(_apontamentoAppService.Object);
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
            _apontamentoAppService.Setup(x => x.Remover(id));

            RemoverModel pageModel = new RemoverModel(_apontamentoAppService.Object);
            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
