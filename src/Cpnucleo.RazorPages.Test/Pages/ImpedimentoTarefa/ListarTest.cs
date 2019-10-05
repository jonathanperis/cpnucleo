using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages.ImpedimentoTarefa;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.ImpedimentoTarefa
{
    public class ListarTest
    {
        private readonly Mock<IImpedimentoTarefaAppService> _impedimentoTarefaAppService;

        public ListarTest()
        {
            _impedimentoTarefaAppService = new Mock<IImpedimentoTarefaAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid idTarefa = Guid.NewGuid();

            List<ImpedimentoTarefaViewModel> listaMock = new List<ImpedimentoTarefaViewModel> { };

            ListarModel pageModel = new ListarModel(_impedimentoTarefaAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _impedimentoTarefaAppService.Setup(x => x.ListarPorTarefa(idTarefa)).Returns(listaMock);

            PageModelTester<ListarModel> pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(idTarefa))

                // Assert
                .TestPage();
        }
    }
}