using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages.Sistema;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Sistema
{
    public class ListarTest
    {
        private readonly Mock<ISistemaAppService> _sistemaAppService;

        public ListarTest()
        {
            _sistemaAppService = new Mock<ISistemaAppService>();
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
