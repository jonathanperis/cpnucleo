using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Workflow;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Workflow
{
    public class ListarTest
    {
        private readonly Mock<IWorkflowRepository> _workflowRepository;

        public ListarTest() => _workflowRepository = new Mock<IWorkflowRepository>();

        [Fact]
        public async Task Test_OnGetAsync()
        {
            // Arrange
            var listaMock = new List<WorkflowItem> { };

            _workflowRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var pageModel = new ListarModel(_workflowRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }
    }
}
