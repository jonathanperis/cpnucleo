using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages
{
    public class FluxoTrabalhoTest
    {
        private readonly Mock<IWorkflowAppService> _workflowAppService;
        private readonly Mock<ITarefaAppService> _tarefaAppService;

        public FluxoTrabalhoTest()
        {
            _workflowAppService = new Mock<IWorkflowAppService>();
            _tarefaAppService = new Mock<ITarefaAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            List<WorkflowViewModel> listaMock = new List<WorkflowViewModel> { };

            FluxoTrabalhoModel pageModel = new FluxoTrabalhoModel(_workflowAppService.Object, _tarefaAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _workflowAppService.Setup(x => x.ListarPorTarefa()).Returns(listaMock);

            PageModelTester<FluxoTrabalhoModel> pageTester = new PageModelTester<FluxoTrabalhoModel>(pageModel);

            // Act
            pageTester

                // Assert
                .Action(x => x.OnGet)
                .TestPage();
        }

        [Fact]
        public void Test_OnPost()
        {
            // Arrange
            Guid idTarefa = Guid.NewGuid();
            Guid idWorkflow = Guid.NewGuid();

            FluxoTrabalhoModel pageModel = new FluxoTrabalhoModel(_workflowAppService.Object, _tarefaAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _tarefaAppService.Setup(x => x.AlterarPorWorkflow(idTarefa, idWorkflow));

            PageModelTester<FluxoTrabalhoModel> pageTester = new PageModelTester<FluxoTrabalhoModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnPost(idTarefa, idWorkflow))

                // Assert
                .TestPage();
        }
    }
}
