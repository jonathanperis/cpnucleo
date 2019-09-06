using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Workflow;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Workflow
{
    public class RemoverTest
    {
        private readonly Mock<IWorkflowRepository> _workflowRepository;

        public RemoverTest() => _workflowRepository = new Mock<IWorkflowRepository>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGetAsync(int idWorkflow)
        {
            // Arrange
            var workflowMock = new WorkflowModel { };

            _workflowRepository.Setup(x => x.ConsultarAsync(idWorkflow)).ReturnsAsync(workflowMock);

            var pageModel = new RemoverModel(_workflowRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGetAsync(idWorkflow))

                // Assert
                .TestPage();
        }

        [Fact]
        public void Test_OnPostAsync()
        {
            // Arrange
            var workflowMock = new WorkflowModel { };

            _workflowRepository.Setup(x => x.RemoverAsync(workflowMock));

            var pageModel = new RemoverModel(_workflowRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPostAsync)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
