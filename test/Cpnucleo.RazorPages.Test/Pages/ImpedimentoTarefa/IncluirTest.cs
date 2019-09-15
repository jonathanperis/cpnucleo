using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.ImpedimentoTarefa;
using Cpnucleo.Application.Interfaces;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System.Collections.Generic;
using Xunit;
using System;

namespace Cpnucleo.RazorPages.Test.Pages.ImpedimentoTarefa
{
    public class IncluirTest
    {
        private readonly Mock<IImpedimentoTarefaAppService> _impedimentoTarefaAppService;
        private readonly Mock<IAppService<ImpedimentoViewModel>> _impedimentoAppService;
        private readonly Mock<ITarefaAppService> _tarefaAppService;

        public IncluirTest()
        {
            _impedimentoTarefaAppService = new Mock<IImpedimentoTarefaAppService>();
            _impedimentoAppService = new Mock<IAppService<ImpedimentoViewModel>>();
            _tarefaAppService = new Mock<ITarefaAppService>();
        }

        [Theory]
        [InlineData(1, 1)]
        public void Test_OnGet(Guid id, Guid idTarefa)
        {
            // Arrange
            var impedimentoTarefaMock = new ImpedimentoTarefaViewModel { };
            var listaMock = new List<ImpedimentoViewModel> { };
            var tarefaMock = new TarefaViewModel { };

            _impedimentoTarefaAppService.Setup(x => x.Consultar(id)).Returns(impedimentoTarefaMock);
            _impedimentoAppService.Setup(x => x.Listar()).Returns(listaMock);
            _tarefaAppService.Setup(x => x.Consultar(idTarefa)).Returns(tarefaMock);

            var pageModel = new IncluirModel(_impedimentoTarefaAppService.Object, _impedimentoAppService.Object, _tarefaAppService.Object);

            var pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(idTarefa))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1, 1, "Descrição do Impedimento")]
        public void Test_OnPost(Guid id, Guid idTarefa, string descricao)
        {
            // Arrange
            var impedimentoTarefaMock = new ImpedimentoTarefaViewModel { Id = id, IdTarefa = idTarefa, Descricao = descricao };
            var listaMock = new List<ImpedimentoViewModel> { };
            var tarefaMock = new TarefaViewModel { };

            _impedimentoTarefaAppService.Setup(x => x.Incluir(impedimentoTarefaMock));
            _impedimentoAppService.Setup(x => x.Listar()).Returns(listaMock);
            _tarefaAppService.Setup(x => x.Consultar(idTarefa)).Returns(tarefaMock);

            var pageModel = new IncluirModel(_impedimentoTarefaAppService.Object, _impedimentoAppService.Object, _tarefaAppService.Object);

            var pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnPost(idTarefa))

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => () => x.OnPost(idTarefa))

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(impedimentoTarefaMock).ShouldReturn.NoErrors();
        }
    }
}
