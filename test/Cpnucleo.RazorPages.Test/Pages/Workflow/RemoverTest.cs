using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Workflow;
using Cpnucleo.Application.Interfaces;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;
using System;

namespace Cpnucleo.RazorPages.Test.Pages.Workflow
{
    public class RemoverTest
    {
        private readonly Mock<IWorkflowAppService> _workflowAppService;

        public RemoverTest() => _workflowAppService = new Mock<IWorkflowAppService>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            var workflowMock = new WorkflowViewModel { };

            _workflowAppService.Setup(x => x.Consultar(id)).Returns(workflowMock);

            var pageModel = new RemoverModel(_workflowAppService.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

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
            var workflowMock = new WorkflowViewModel { };

            _workflowAppService.Setup(x => x.Remover(workflowMock));

            var pageModel = new RemoverModel(_workflowAppService.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
