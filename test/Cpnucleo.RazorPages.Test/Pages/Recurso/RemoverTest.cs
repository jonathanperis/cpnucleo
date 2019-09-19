using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Recurso;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Recurso
{
    public class RemoverTest
    {
        private readonly Mock<IRecursoAppService> _recursoAppService;

        public RemoverTest()
        {
            _recursoAppService = new Mock<IRecursoAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = new Guid();

            RecursoViewModel recursoMock = new RecursoViewModel { };

            _recursoAppService.Setup(x => x.Consultar(id)).Returns(recursoMock);

            RemoverModel pageModel = new RemoverModel(_recursoAppService.Object);
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

            _recursoAppService.Setup(x => x.Remover(id));

            RemoverModel pageModel = new RemoverModel(_recursoAppService.Object)
            {
                Recurso = new RecursoViewModel { Id = id }
            };

            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
