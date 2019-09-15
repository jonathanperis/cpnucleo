using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Sistema;
using Cpnucleo.Application.Interfaces;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Sistema
{
    public class ListarTest
    {
        private readonly Mock<IAppService<SistemaViewModel>> _sistemaAppService;

        public ListarTest() => _sistemaAppService = new Mock<IAppService<SistemaViewModel>>();

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            var listaMock = new List<SistemaViewModel> { };

            _sistemaAppService.Setup(x => x.Listar()).Returns(listaMock);

            var pageModel = new ListarModel(_sistemaAppService.Object);

            var pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester

                // Assert
                .Action(x => x.OnGet)
                .TestPage();
        }
    }
}
