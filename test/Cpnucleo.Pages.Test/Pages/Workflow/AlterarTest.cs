using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Workflow;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

            var pageModel = new AlterarModel(_workflowRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnGetAsync(idWorkflow);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(129, "Workflow de Teste", 3)]
        public async Task Test_OnPostAsync(int idWorkflow, string nome, int ordem)
        {
            // Arrange
            var workflowMock = new WorkflowItem { IdWorkflow = idWorkflow, Nome = nome, Ordem = ordem };

            _workflowRepository.Setup(x => x.AlterarAsync(workflowMock));

            var pageModel = new AlterarModel(_workflowRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}
