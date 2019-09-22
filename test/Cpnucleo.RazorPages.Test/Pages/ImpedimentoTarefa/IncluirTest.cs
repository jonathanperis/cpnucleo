using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.ImpedimentoTarefa;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System;
using System.Collections.Generic;
using Xunit;

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

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            Guid idTarefa = Guid.NewGuid();

            ImpedimentoTarefaViewModel impedimentoTarefaMock = new ImpedimentoTarefaViewModel { };
            List<ImpedimentoViewModel> listaMock = new List<ImpedimentoViewModel> { };
            TarefaViewModel tarefaMock = new TarefaViewModel { };

            _impedimentoTarefaAppService.Setup(x => x.Consultar(id)).Returns(impedimentoTarefaMock);
            _impedimentoAppService.Setup(x => x.Listar()).Returns(listaMock);
            _tarefaAppService.Setup(x => x.Consultar(idTarefa)).Returns(tarefaMock);

            IncluirModel pageModel = new IncluirModel(_impedimentoTarefaAppService.Object, _impedimentoAppService.Object, _tarefaAppService.Object);
            PageModelTester<IncluirModel> pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(idTarefa))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData("Descrição do Impedimento")]
        public void Test_OnPost(string descricao)
        {
            // Arrange
            Guid id = Guid.NewGuid();
            Guid idTarefa = Guid.NewGuid();

            ImpedimentoTarefaViewModel impedimentoTarefaMock = new ImpedimentoTarefaViewModel { Id = id, IdTarefa = idTarefa, Descricao = descricao };
            List<ImpedimentoViewModel> listaMock = new List<ImpedimentoViewModel> { };
            TarefaViewModel tarefaMock = new TarefaViewModel { };

            _impedimentoTarefaAppService.Setup(x => x.Incluir(impedimentoTarefaMock));
            _impedimentoAppService.Setup(x => x.Listar()).Returns(listaMock);
            _tarefaAppService.Setup(x => x.Consultar(idTarefa)).Returns(tarefaMock);

            IncluirModel pageModel = new IncluirModel(_impedimentoTarefaAppService.Object, _impedimentoAppService.Object, _tarefaAppService.Object);
            PageModelTester<IncluirModel> pageTester = new PageModelTester<IncluirModel>(pageModel);

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
