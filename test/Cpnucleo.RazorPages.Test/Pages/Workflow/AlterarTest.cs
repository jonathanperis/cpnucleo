using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Workflow;
using Cpnucleo.Application.Interfaces;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;
using System;

namespace Cpnucleo.RazorPages.Test.Pages.Workflow
{
    public class AlterarTest
    {
        private readonly Mock<IWorkflowAppService> _workflowAppService;

        public AlterarTest() => _workflowAppService = new Mock<IWorkflowAppService>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            var workflowMock = new WorkflowViewModel { };

            _workflowAppService.Setup(x => x.Consultar(id)).Returns(workflowMock);

            var pageModel = new AlterarModel(_workflowAppService.Object);

            var pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(129, "Workflow de Teste", 3)]
        public void Test_OnPost(Guid id, string nome, int ordem)
        {
            // Arrange
            var workflowMock = new WorkflowViewModel { Id = id, Nome = nome, Ordem = ordem };

            _workflowAppService.Setup(x => x.Alterar(workflowMock));

            var pageModel = new AlterarModel(_workflowAppService.Object);

            var pageTester = new PageModelTester<AlterarModel>(pageModel);

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
