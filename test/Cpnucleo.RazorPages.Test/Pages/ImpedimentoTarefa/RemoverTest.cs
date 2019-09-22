using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.ImpedimentoTarefa;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.ImpedimentoTarefa
{
    public class RemoverTest
    {
        private readonly Mock<IImpedimentoTarefaAppService> _impedimentoTarefaAppService;

        public RemoverTest()
        {
            _impedimentoTarefaAppService = new Mock<IImpedimentoTarefaAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            ImpedimentoTarefaViewModel impedimentoTarefaMock = new ImpedimentoTarefaViewModel { };

            _impedimentoTarefaAppService.Setup(x => x.Consultar(id)).Returns(impedimentoTarefaMock);

            RemoverModel pageModel = new RemoverModel(_impedimentoTarefaAppService.Object);
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

            _impedimentoTarefaAppService.Setup(x => x.Remover(id));

            RemoverModel pageModel = new RemoverModel(_impedimentoTarefaAppService.Object)
            {
                ImpedimentoTarefa = new ImpedimentoTarefaViewModel { Id = id, IdTarefa = idTarefa }
            };

            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
