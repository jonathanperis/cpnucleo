using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Workflow;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Workflow
{
    public class IncluirTest
    {
        private readonly Mock<IWorkflowRepository> _workflowRepository;

        public IncluirTest() => _workflowRepository = new Mock<IWorkflowRepository>();

        [Theory]
        [InlineData("Workflow de Teste", 3)]
        public void Test_OnPostAsync(string nome, int ordem)
        {
            // Arrange
            var workflowMock = new WorkflowModel { Nome = nome, Ordem = ordem };

            _workflowRepository.Setup(x => x.IncluirAsync(workflowMock));

            var pageModel = new IncluirModel(_workflowRepository.Object);

            var pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPostAsync)

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => x.OnPostAsync)

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(workflowMock).ShouldReturn.NoErrors();
        }
    }
}
