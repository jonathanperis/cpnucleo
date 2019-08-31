using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Workflow;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

            var pageModel = new IncluirModel(_workflowRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }
    }
}
