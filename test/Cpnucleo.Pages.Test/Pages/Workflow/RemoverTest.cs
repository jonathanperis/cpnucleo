using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Workflow;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Workflow
{
    public class RemoverTest
    {
        private readonly Mock<IWorkflowRepository> _workflowRepository;

        public RemoverTest() => _workflowRepository = new Mock<IWorkflowRepository>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idWorkflow)
        {
            // Arrange
            var workflowMock = new WorkflowItem { };

            _workflowRepository.Setup(x => x.ConsultarAsync(idWorkflow)).ReturnsAsync(workflowMock);

            var RemoverModel = new RemoverModel(_workflowRepository.Object);

            // Act
            var actionResult = await RemoverModel.OnGetAsync(idWorkflow);

            // Assert
            Assert.NotNull(actionResult);
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnPostAsync(int idWorkflow)
        {
            // Arrange
            var workflowMock = new WorkflowItem { IdWorkflow = idWorkflow };

            _workflowRepository.Setup(x => x.RemoverAsync(workflowMock));

            var removerModel = new RemoverModel(_workflowRepository.Object);

            // Act
            var actionResult = await removerModel.OnPostAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
