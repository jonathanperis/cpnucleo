using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Workflow;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Workflow
{
    public class ListarTest
    {
        private readonly Mock<IWorkflowAppService> _workflowAppService;

        public ListarTest()
        {
            _workflowAppService = new Mock<IWorkflowAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            List<WorkflowViewModel> listaMock = new List<WorkflowViewModel> { };

            _workflowAppService.Setup(x => x.Listar()).Returns(listaMock);

            ListarModel pageModel = new ListarModel(_workflowAppService.Object);
            PageModelTester<ListarModel> pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester

                // Assert
                .Action(x => x.OnGet)
                .TestPage();
        }
    }
}
