using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Sistema;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Sistema
{
    public class ListarTest
    {
        private readonly Mock<ICrudAppService<SistemaViewModel>> _sistemaAppService;

        public ListarTest()
        {
            _sistemaAppService = new Mock<ICrudAppService<SistemaViewModel>>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            List<SistemaViewModel> listaMock = new List<SistemaViewModel> { };

            _sistemaAppService.Setup(x => x.Listar()).Returns(listaMock);

            ListarModel pageModel = new ListarModel(_sistemaAppService.Object);
            PageModelTester<ListarModel> pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester

                // Assert
                .Action(x => x.OnGet)
                .TestPage();
        }
    }
}
