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

        public RemoverTest()
        {
            _recursoTarefaAppService = new Mock<IRecursoTarefaAppService>();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            RecursoTarefaViewModel recursoTarefaMock = new RecursoTarefaViewModel { };

            _recursoTarefaAppService.Setup(x => x.Consultar(id)).Returns(recursoTarefaMock);

            RemoverModel pageModel = new RemoverModel(_recursoTarefaAppService.Object);
            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnPost(Guid id)
        {
            // Arrange
            _recursoTarefaAppService.Setup(x => x.Remover(id));

            RemoverModel pageModel = new RemoverModel(_recursoTarefaAppService.Object);
            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnPost())

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
