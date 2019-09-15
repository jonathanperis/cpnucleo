using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Impedimento;
using Cpnucleo.Application.Interfaces;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Impedimento
{
    public class ListarTest
    {
        private readonly Mock<IAppService<ImpedimentoViewModel>> _impedimentoAppService;

        public ListarTest() => _impedimentoAppService = new Mock<IAppService<ImpedimentoViewModel>>();

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            var listaMock = new List<ImpedimentoViewModel> { };

            _impedimentoAppService.Setup(x => x.Listar()).Returns(listaMock);

            var pageModel = new ListarModel(_impedimentoAppService.Object);

            var pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester

                // Assert
                .Action(x => x.OnGet)
                .TestPage();
        }
    }
}
