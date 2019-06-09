using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Workflow;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Workflow
{
    public class IncluirTest
    {
        private readonly Mock<IWorkflowRepository> _workflowRepository;

        public IncluirTest() => _workflowRepository = new Mock<IWorkflowRepository>();

        [Theory]
        [InlineData("Workflow de Teste", 3)]
        public async Task Test_OnPostAsync(string nome, int ordem)
        {
            // Arrange
            var workflowMock = new WorkflowItem { Nome = nome, Ordem = ordem };

            _workflowRepository.Setup(x => x.IncluirAsync(workflowMock));

            var incluirModel = new IncluirModel(_workflowRepository.Object);

            // Act
            var actionResult = await incluirModel.OnPostAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
