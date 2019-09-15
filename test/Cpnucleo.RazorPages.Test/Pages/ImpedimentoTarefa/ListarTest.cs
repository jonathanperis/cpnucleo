using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.ImpedimentoTarefa;
using Cpnucleo.Application.Interfaces;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System.Collections.Generic;
using Xunit;
using System;

namespace Cpnucleo.RazorPages.Test.Pages.ImpedimentoTarefa
{
    public class ListarTest
    {
        private readonly Mock<IImpedimentoTarefaAppService> _impedimentoTarefaAppService;

        public ListarTest() => _impedimentoTarefaAppService = new Mock<IImpedimentoTarefaAppService>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid idTarefa)
        {
            // Arrange
            var listaMock = new List<ImpedimentoTarefaViewModel> { };

            _impedimentoTarefaAppService.Setup(x => x.ListarPorTarefa(idTarefa)).Returns(listaMock);

            var pageModel = new ListarModel(_impedimentoTarefaAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            var pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(idTarefa))

                // Assert
                .TestPage();
        }
    }
}