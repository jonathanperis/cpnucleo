using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages.Impedimento;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Impedimento
{
    public class RemoverTest
    {
        private readonly Mock<IImpedimentoAppService> _impedimentoAppService;

        public RemoverTest()
        {
            _impedimentoAppService = new Mock<IImpedimentoAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            ImpedimentoViewModel impedimentoMock = new ImpedimentoViewModel { };

            RemoverModel pageModel = new RemoverModel(_impedimentoAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _impedimentoAppService.Setup(x => x.Consultar(id)).Returns(impedimentoMock);

            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Fact]
        public void Test_OnPost()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            ImpedimentoViewModel impedimentoMock = new ImpedimentoViewModel { };

            RemoverModel pageModel = new RemoverModel(_impedimentoAppService.Object)
            {
                Impedimento = new ImpedimentoViewModel { Id = id },
                PageContext = PageContextManager.CreatePageContext()
            };

            _impedimentoAppService.Setup(x => x.Remover(id));

            PageModelTester<RemoverModel> pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
