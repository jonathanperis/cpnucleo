using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
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
        private readonly Mock<IImpedimentoAppService> _impedimentoAppService;

        public AlterarTest()
        {
            _impedimentoTarefaAppService = new Mock<IImpedimentoTarefaAppService>();
            _impedimentoAppService = new Mock<IImpedimentoAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            ImpedimentoTarefaViewModel impedimentoTarefaMock = new ImpedimentoTarefaViewModel { };
            List<ImpedimentoViewModel> listaMock = new List<ImpedimentoViewModel> { };

            AlterarModel pageModel = new AlterarModel(_impedimentoTarefaAppService.Object, _impedimentoAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _impedimentoTarefaAppService.Setup(x => x.Consultar(id)).Returns(impedimentoTarefaMock);
            _impedimentoAppService.Setup(x => x.Listar()).Returns(listaMock);

            PageModelTester<AlterarModel> pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData("Descrição do Impedimento")]
        public void Test_OnPost(string descricao)
        {
            // Arrange
            Guid id = Guid.NewGuid();
            Guid idImpedimento = Guid.NewGuid();
            Guid idTarefa = Guid.NewGuid();

            ImpedimentoTarefaViewModel impedimentoTarefaMock = new ImpedimentoTarefaViewModel { Id = id, IdImpedimento = idImpedimento, IdTarefa = idTarefa, Descricao = descricao };
            List<ImpedimentoViewModel> listaMock = new List<ImpedimentoViewModel> { };

            AlterarModel pageModel = new AlterarModel(_impedimentoTarefaAppService.Object, _impedimentoAppService.Object)
            {
                ImpedimentoTarefa = new ImpedimentoTarefaViewModel { IdTarefa = idTarefa },
                PageContext = PageContextManager.CreatePageContext()
            };

            _impedimentoTarefaAppService.Setup(x => x.Alterar(impedimentoTarefaMock));
            _impedimentoAppService.Setup(x => x.Listar()).Returns(listaMock);

            PageModelTester<AlterarModel> pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(impedimentoTarefaMock).ShouldReturn.NoErrors();
        }
    }
}
