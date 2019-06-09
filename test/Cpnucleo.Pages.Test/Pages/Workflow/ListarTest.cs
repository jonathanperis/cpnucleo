using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Workflow;
using Cpnucleo.Pages.Repository;
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
            var listarModel = new ListarModel(_workflowRepository.Object);

            // Act
            var actionResult = await listarModel.OnGetAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
