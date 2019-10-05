using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages.Projeto;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Projeto
{
    public class ListarTest
    {
        private readonly Mock<IProjetoAppService> _projetoAppService;

        public ListarTest()
        {
            _projetoAppService = new Mock<IProjetoAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            List<ProjetoViewModel> listaMock = new List<ProjetoViewModel> { };

            ListarModel pageModel = new ListarModel(_projetoAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _projetoAppService.Setup(x => x.Listar()).Returns(listaMock);

            PageModelTester<ListarModel> pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester

                // Assert
                .Action(x => x.OnGet)
                .TestPage();
        }
    }
}
