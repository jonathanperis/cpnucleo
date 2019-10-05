using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages.Impedimento;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Impedimento
{
    public class AlterarTest
    {
        private readonly Mock<IImpedimentoAppService> _impedimentoAppService;

        public AlterarTest()
        {
            _impedimentoAppService = new Mock<IImpedimentoAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            ImpedimentoViewModel impedimentoMock = new ImpedimentoViewModel { };

            _impedimentoAppService.Setup(x => x.Consultar(id)).Returns(impedimentoMock);

            AlterarModel pageModel = new AlterarModel(_impedimentoAppService.Object);
            PageModelTester<AlterarModel> pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData("Impedimento de Teste")]
        public void Test_OnPost(string nome)
        {
            // Arrange
            Guid id = Guid.NewGuid();

            ImpedimentoViewModel impedimentoMock = new ImpedimentoViewModel { Id = id, Nome = nome };

            _impedimentoAppService.Setup(x => x.Alterar(impedimentoMock));

            AlterarModel pageModel = new AlterarModel(_impedimentoAppService.Object);
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
            Validation.For(impedimentoMock).ShouldReturn.NoErrors();
        }
    }
}
