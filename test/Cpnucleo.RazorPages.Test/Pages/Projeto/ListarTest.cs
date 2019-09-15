using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Projeto;
using Cpnucleo.Application.Interfaces;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Projeto
{
    public class ListarTest
    {
        private readonly Mock<IAppService<ProjetoViewModel>> _projetoAppService;

        public ListarTest() => _projetoAppService = new Mock<IAppService<ProjetoViewModel>>();

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            var listaMock = new List<ProjetoViewModel> { };

            _projetoAppService.Setup(x => x.Listar()).Returns(listaMock);

            var pageModel = new ListarModel(_projetoAppService.Object);

            var pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester

                // Assert
                .Action(x => x.OnGet)
                .TestPage();
        }
    }
}
