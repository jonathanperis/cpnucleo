using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Workflow;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Workflow
{
    public class RemoverTest
    {
        private readonly Mock<IWorkflowAppService> _workflowAppService;

        public RemoverTest()
        {
            _workflowAppService = new Mock<IWorkflowAppService>();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            WorkflowViewModel workflowMock = new WorkflowViewModel { };

            _workflowAppService.Setup(x => x.Consultar(id)).Returns(workflowMock);

            RemoverModel pageModel = new RemoverModel(_workflowAppService.Object);
            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnPost(Guid id)
        {
            // Arrange
            WorkflowViewModel workflowMock = new WorkflowViewModel { };

            _workflowAppService.Setup(x => x.Remover(id));

            RemoverModel pageModel = new RemoverModel(_workflowAppService.Object);
            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
