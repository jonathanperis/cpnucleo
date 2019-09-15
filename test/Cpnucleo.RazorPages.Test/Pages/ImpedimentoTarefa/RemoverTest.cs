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

        public RemoverTest() => _impedimentoTarefaAppService = new Mock<IImpedimentoTarefaAppService>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            var impedimentoTarefaMock = new ImpedimentoTarefaViewModel { };

            _impedimentoTarefaAppService.Setup(x => x.Consultar(id)).Returns(impedimentoTarefaMock);

            var pageModel = new RemoverModel(_impedimentoTarefaAppService.Object);

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
            var impedimentoTarefaMock = new ImpedimentoTarefaViewModel { };

            _impedimentoTarefaAppService.Setup(x => x.Remover(impedimentoTarefaMock));

            var pageModel = new RemoverModel(_impedimentoTarefaAppService.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnPost())

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
