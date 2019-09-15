using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Workflow;
using Cpnucleo.Application.Interfaces;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Workflow
{
    public class IncluirTest
    {
        private readonly Mock<IWorkflowAppService> _workflowAppService;

        public IncluirTest() => _workflowAppService = new Mock<IWorkflowAppService>();

        [Theory]
        [InlineData("Workflow de Teste", 3)]
        public void Test_OnPost(string nome, int ordem)
        {
            // Arrange
            var workflowMock = new WorkflowViewModel { Nome = nome, Ordem = ordem };

            _workflowAppService.Setup(x => x.Incluir(workflowMock));

            var pageModel = new IncluirModel(_workflowAppService.Object);

            var pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(workflowMock).ShouldReturn.NoErrors();
        }
    }
}
