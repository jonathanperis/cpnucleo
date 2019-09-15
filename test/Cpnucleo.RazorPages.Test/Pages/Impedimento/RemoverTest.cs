using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Impedimento;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Impedimento
{
    public class RemoverTest
    {
        private readonly Mock<IAppService<ImpedimentoViewModel>> _impedimentoAppService;

        public RemoverTest()
        {
            _impedimentoAppService = new Mock<IAppService<ImpedimentoViewModel>>();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            ImpedimentoViewModel impedimentoMock = new ImpedimentoViewModel { };

            _impedimentoAppService.Setup(x => x.Consultar(id)).Returns(impedimentoMock);

            RemoverModel pageModel = new RemoverModel(_impedimentoAppService.Object);
            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnPost(Guid id)
        {
            // Arrange
            ImpedimentoViewModel impedimentoMock = new ImpedimentoViewModel { };

            _impedimentoAppService.Setup(x => x.Remover(id));

            RemoverModel pageModel = new RemoverModel(_impedimentoAppService.Object);
            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
