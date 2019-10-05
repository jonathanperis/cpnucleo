using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
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
            Guid id = Guid.NewGuid();

            TarefaViewModel tarefaMock = new TarefaViewModel { };

            RemoverModel pageModel = new RemoverModel(_tarefaAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _tarefaAppService.Setup(x => x.Consultar(id)).Returns(tarefaMock);

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
            Guid id = Guid.NewGuid();

            TarefaViewModel tarefaMock = new TarefaViewModel { };

            RemoverModel pageModel = new RemoverModel(_tarefaAppService.Object)
            {
                Tarefa = new TarefaViewModel { Id = id },
                PageContext = PageContextManager.CreatePageContext()
            };

            _tarefaAppService.Setup(x => x.Remover(id));

            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
