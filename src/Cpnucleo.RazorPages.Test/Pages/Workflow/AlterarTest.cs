using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages.Workflow;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Workflow
{
    public class AlterarTest
    {
        private readonly Mock<IWorkflowAppService> _workflowAppService;

        public AlterarTest()
        {
            _workflowAppService = new Mock<IWorkflowAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            WorkflowViewModel workflowMock = new WorkflowViewModel { };

            AlterarModel pageModel = new AlterarModel(_workflowAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _workflowAppService.Setup(x => x.Consultar(id)).Returns(workflowMock);

            PageModelTester<AlterarModel> pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData("Workflow de Teste", 3)]
        public void Test_OnPost(string nome, int ordem)
        {
            // Arrange
            Guid id = Guid.NewGuid();

            WorkflowViewModel workflowMock = new WorkflowViewModel { Id = id, Nome = nome, Ordem = ordem };

            AlterarModel pageModel = new AlterarModel(_workflowAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _workflowAppService.Setup(x => x.Alterar(workflowMock));

            PageModelTester<AlterarModel> pageTester = new PageModelTester<AlterarModel>(pageModel);

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
