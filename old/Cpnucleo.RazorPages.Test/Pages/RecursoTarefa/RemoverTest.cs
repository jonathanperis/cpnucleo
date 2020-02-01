using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
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
            Guid id = Guid.NewGuid();

            RecursoTarefaViewModel recursoTarefaMock = new RecursoTarefaViewModel { };

            RemoverModel pageModel = new RemoverModel(_recursoTarefaAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _recursoTarefaAppService.Setup(x => x.Consultar(id)).Returns(recursoTarefaMock);

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
            Guid idTarefa = Guid.NewGuid();

            RemoverModel pageModel = new RemoverModel(_recursoTarefaAppService.Object)
            {
                RecursoTarefa = new RecursoTarefaViewModel { Id = id, IdTarefa = idTarefa },
                PageContext = PageContextManager.CreatePageContext()
            };

            _recursoTarefaAppService.Setup(x => x.Remover(id));

            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
