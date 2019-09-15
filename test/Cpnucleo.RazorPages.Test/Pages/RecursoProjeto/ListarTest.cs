using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.RecursoProjeto;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.RecursoProjeto
{
    public class ListarTest
    {
        private readonly Mock<IRecursoProjetoAppService> _recursoProjetoAppService;

        public ListarTest() => _recursoProjetoAppService = new Mock<IRecursoProjetoAppService>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid idProjeto)
        {
            // Arrange
            var listaMock = new List<RecursoProjetoViewModel> { };

            _recursoProjetoAppService.Setup(x => x.Listar()).Returns(listaMock);

            var pageModel = new ListarModel(_recursoProjetoAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            var pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(idProjeto))

                // Assert
                .TestPage();
        }
    }
}