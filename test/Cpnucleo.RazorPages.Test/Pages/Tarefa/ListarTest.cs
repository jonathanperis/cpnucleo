using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Tarefa;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Tarefa
{
    public class ListarTest
    {
        private readonly Mock<ITarefaAppService> _tarefaAppService;

        public ListarTest() => _tarefaAppService = new Mock<ITarefaAppService>();

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            var listaMock = new List<TarefaViewModel> { };

            _tarefaAppService.Setup(x => x.Listar()).Returns(listaMock);

            var pageModel = new ListarModel(_tarefaAppService.Object);

            var pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester

                // Assert
                .Action(x => x.OnGet)
                .TestPage();
        }
    }
}
