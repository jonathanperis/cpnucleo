using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.RecursoTarefa;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.RecursoTarefa
{
    public class RemoverTest
    {
        private readonly Mock<IRecursoTarefaAppService> _recursoTarefaAppService;
        private readonly Mock<ITarefaAppService> _tarefaAppService;

        public RemoverTest()
        {
            _recursoTarefaAppService = new Mock<IRecursoTarefaAppService>();
            _tarefaAppService = new Mock<ITarefaAppService>();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            var recursoTarefaMock = new RecursoTarefaViewModel { };

            _recursoTarefaAppService.Setup(x => x.Consultar(id)).Returns(recursoTarefaMock);

            var pageModel = new RemoverModel(_recursoTarefaAppService.Object, _tarefaAppService.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnPost(Guid idTarefa)
        {
            // Arrange
            var recursoTarefaMock = new RecursoTarefaViewModel { };

            _recursoTarefaAppService.Setup(x => x.Remover(recursoTarefaMock));

            var pageModel = new RemoverModel(_recursoTarefaAppService.Object, _tarefaAppService.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnPost())

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
