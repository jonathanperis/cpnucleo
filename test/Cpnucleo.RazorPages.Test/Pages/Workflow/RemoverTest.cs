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

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = Guid.NewGuid();

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

        [Fact]
        public void Test_OnPost()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            WorkflowViewModel workflowMock = new WorkflowViewModel { };

            _workflowAppService.Setup(x => x.Remover(id));

            RemoverModel pageModel = new RemoverModel(_workflowAppService.Object)
            {
                Workflow = new WorkflowViewModel { Id = id }
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
