using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Tarefa;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Tarefa
{
    public class RemoverTest
    {
        private readonly Mock<ITarefaAppService> _tarefaAppService;

        public RemoverTest() => _tarefaAppService = new Mock<ITarefaAppService>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            var tarefaMock = new TarefaViewModel { };

            _tarefaAppService.Setup(x => x.Consultar(id)).Returns(tarefaMock);

            var pageModel = new RemoverModel(_tarefaAppService.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Fact]
        public void Test_OnPost()
        {
            // Arrange
            var tarefaMock = new TarefaViewModel { };

            _tarefaAppService.Setup(x => x.Remover(tarefaMock));

            var pageModel = new RemoverModel(_tarefaAppService.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
