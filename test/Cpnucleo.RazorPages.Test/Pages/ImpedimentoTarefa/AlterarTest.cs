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
    public class AlterarTest
    {
        private readonly Mock<IImpedimentoTarefaAppService> _impedimentoTarefaAppService;
        private readonly Mock<IAppService<ImpedimentoViewModel>> _impedimentoAppService;

        public AlterarTest()
        {
            _impedimentoTarefaAppService = new Mock<IImpedimentoTarefaAppService>();
            _impedimentoAppService = new Mock<IAppService<ImpedimentoViewModel>>();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            ImpedimentoTarefaViewModel impedimentoTarefaMock = new ImpedimentoTarefaViewModel { };
            List<ImpedimentoViewModel> listaMock = new List<ImpedimentoViewModel> { };

            _impedimentoTarefaAppService.Setup(x => x.Consultar(id)).Returns(impedimentoTarefaMock);
            _impedimentoAppService.Setup(x => x.Listar()).Returns(listaMock);

            AlterarModel pageModel = new AlterarModel(_impedimentoTarefaAppService.Object, _impedimentoAppService.Object);
            PageModelTester<AlterarModel> pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1, 1, 1, "Descrição do Impedimento")]
        public void Test_OnPost(Guid id, Guid idImpedimento, Guid idTarefa, string descricao)
        {
            // Arrange
            ImpedimentoTarefaViewModel impedimentoTarefaMock = new ImpedimentoTarefaViewModel { Id = id, IdImpedimento = idImpedimento, IdTarefa = idTarefa, Descricao = descricao };
            List<ImpedimentoViewModel> listaMock = new List<ImpedimentoViewModel> { };

            _impedimentoTarefaAppService.Setup(x => x.Alterar(impedimentoTarefaMock));
            _impedimentoAppService.Setup(x => x.Listar()).Returns(listaMock);

            AlterarModel pageModel = new AlterarModel(_impedimentoTarefaAppService.Object, _impedimentoAppService.Object);
            PageModelTester<AlterarModel> pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnPost())

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => () => x.OnPost())

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(impedimentoTarefaMock).ShouldReturn.NoErrors();
        }
    }
}
