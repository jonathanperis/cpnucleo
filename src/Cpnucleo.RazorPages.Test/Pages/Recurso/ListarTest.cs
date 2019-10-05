using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages.Recurso;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Recurso
{
    public class ListarTest
    {
        private readonly Mock<IRecursoAppService> _recursoAppService;

        public ListarTest()
        {
            _recursoAppService = new Mock<IRecursoAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            List<RecursoViewModel> listaMock = new List<RecursoViewModel> { };

            ListarModel pageModel = new ListarModel(_recursoAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _recursoAppService.Setup(x => x.Listar()).Returns(listaMock);

            PageModelTester<ListarModel> pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester

                // Assert
                .Action(x => x.OnGet)
                .TestPage();
        }
    }
}
