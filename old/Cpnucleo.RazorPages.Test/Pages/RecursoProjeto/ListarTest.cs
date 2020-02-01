using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
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

        public ListarTest()
        {
            _recursoProjetoAppService = new Mock<IRecursoProjetoAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid idProjeto = Guid.NewGuid();

            List<RecursoProjetoViewModel> listaMock = new List<RecursoProjetoViewModel> { };

            ListarModel pageModel = new ListarModel(_recursoProjetoAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _recursoProjetoAppService.Setup(x => x.Listar()).Returns(listaMock);

            PageModelTester<ListarModel> pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(idProjeto))

                // Assert
                .TestPage();
        }
    }
}