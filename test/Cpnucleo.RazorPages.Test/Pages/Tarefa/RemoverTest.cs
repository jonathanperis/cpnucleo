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

        public RemoverTest()
        {
            _tarefaAppService = new Mock<ITarefaAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = new Guid();
            TarefaViewModel tarefaMock = new TarefaViewModel { };

            _tarefaAppService.Setup(x => x.Consultar(id)).Returns(tarefaMock);

            RemoverModel pageModel = new RemoverModel(_tarefaAppService.Object);
            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

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
            Guid id = new Guid();
            TarefaViewModel tarefaMock = new TarefaViewModel { };

            _tarefaAppService.Setup(x => x.Remover(id));

            RemoverModel pageModel = new RemoverModel(_tarefaAppService.Object);
            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
