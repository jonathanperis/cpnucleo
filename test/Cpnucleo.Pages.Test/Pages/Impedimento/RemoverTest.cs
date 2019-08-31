using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Impedimento;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Impedimento
{
    public class RemoverTest
    {
        private readonly Mock<IRepository<ImpedimentoItem>> _impedimentoRepository;

        public RemoverTest() => _impedimentoRepository = new Mock<IRepository<ImpedimentoItem>>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idImpedimento)
        {
            // Arrange
            var impedimentoMock = new ImpedimentoItem { };

            _impedimentoRepository.Setup(x => x.ConsultarAsync(idImpedimento)).ReturnsAsync(impedimentoMock);

            var pageModel = new RemoverModel(_impedimentoRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnGetAsync(idImpedimento);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnPostAsync(int idImpedimento)
        {
            // Arrange
            var impedimentoMock = new ImpedimentoItem { IdImpedimento = idImpedimento };

            _impedimentoRepository.Setup(x => x.RemoverAsync(impedimentoMock));

            var pageModel = new RemoverModel(_impedimentoRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }
    }
}
