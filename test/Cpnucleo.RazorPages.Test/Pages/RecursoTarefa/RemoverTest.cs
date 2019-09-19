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

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = new Guid();

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

        [Fact]
        public void Test_OnPost()
        {
            // Arrange
            Guid id = new Guid();
            Guid idTarefa = new Guid();

            _recursoTarefaAppService.Setup(x => x.Remover(id));

            RemoverModel pageModel = new RemoverModel(_recursoTarefaAppService.Object);
            pageModel.RecursoTarefa = new RecursoTarefaViewModel { Id = id, IdTarefa = idTarefa };

            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
