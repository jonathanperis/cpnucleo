using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.RecursoTarefa;
using Cpnucleo.Application.Interfaces;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System.Collections.Generic;
using Xunit;
using System;

namespace Cpnucleo.RazorPages.Test.Pages.RecursoTarefa
{
    public class ListarTest
    {
        private readonly Mock<IRecursoTarefaAppService> _sistemaAppService;

        public ListarTest() => _sistemaAppService = new Mock<IRecursoTarefaAppService>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid idTarefa)
        {
            // Arrange
            var listaMock = new List<RecursoTarefaViewModel> { };

            _sistemaAppService.Setup(x => x.ListarPorTarefa(idTarefa)).Returns(listaMock);

            var pageModel = new ListarModel(_sistemaAppService.Object)
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
