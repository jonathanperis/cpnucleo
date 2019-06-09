using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Workflow;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Workflow
{
    public class AlterarTest
    {
        private readonly Mock<IWorkflowRepository> _workflowRepository;

        public AlterarTest() => _workflowRepository = new Mock<IWorkflowRepository>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idWorkflow)
        {
            // Arrange
            var workflowMock = new WorkflowItem { };

            _workflowRepository.Setup(x => x.ConsultarAsync(idWorkflow)).ReturnsAsync(workflowMock);

            var AlterarModel = new AlterarModel(_workflowRepository.Object);

            // Act
            var actionResult = await AlterarModel.OnGetAsync();

            // Assert
            Assert.NotNull(actionResult);
        }

        [Theory]
        [InlineData(129, "Workflow de Teste", 3)]
        public async Task Test_OnPostAsync(int idWorkflow, string nome, int ordem)
        {
            // Arrange
            var workflowMock = new WorkflowItem { IdWorkflow = idWorkflow, Nome = nome, Ordem = ordem };

            _workflowRepository.Setup(x => x.AlterarAsync(workflowMock));

            var alterarModel = new AlterarModel(_workflowRepository.Object);

            // Act
            var actionResult = await alterarModel.OnPostAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
