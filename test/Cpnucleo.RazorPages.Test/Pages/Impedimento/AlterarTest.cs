using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Impedimento;
using Cpnucleo.Application.Interfaces;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;
using System;

namespace Cpnucleo.RazorPages.Test.Pages.Impedimento
{
    public class AlterarTest
    {
        private readonly Mock<IAppService<ImpedimentoViewModel>> _impedimentoAppService;

        public AlterarTest() => _impedimentoAppService = new Mock<IAppService<ImpedimentoViewModel>>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            var impedimentoMock = new ImpedimentoViewModel { };

            _impedimentoAppService.Setup(x => x.Consultar(id)).Returns(impedimentoMock);

            var pageModel = new AlterarModel(_impedimentoAppService.Object);

            var pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1, "Impedimento de Teste")]
        public void Test_OnPost(Guid id, string nome)
        {
            // Arrange
            var impedimentoMock = new ImpedimentoViewModel { Id = id, Nome = nome };

            _impedimentoAppService.Setup(x => x.Alterar(impedimentoMock));

            var pageModel = new AlterarModel(_impedimentoAppService.Object);

            var pageTester = new PageModelTester<AlterarModel>(pageModel);

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
            Validation.For(impedimentoMock).ShouldReturn.NoErrors();
        }
    }
}
