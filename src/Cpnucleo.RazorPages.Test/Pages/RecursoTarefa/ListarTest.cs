using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages.RecursoTarefa;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.RecursoTarefa
{
    public class ListarTest
    {
        private readonly Mock<IRecursoTarefaAppService> _sistemaAppService;

        public ListarTest()
        {
            _sistemaAppService = new Mock<IRecursoTarefaAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid idTarefa = Guid.NewGuid();

            List<RecursoTarefaViewModel> listaMock = new List<RecursoTarefaViewModel> { };

            ListarModel pageModel = new ListarModel(_sistemaAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _sistemaAppService.Setup(x => x.ListarPorTarefa(idTarefa)).Returns(listaMock);

            PageModelTester<ListarModel> pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(idTarefa))

                // Assert
                .TestPage();
        }
    }
}
