using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Apontamento;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Apontamento
{
    public class RemoverTest
    {
        private readonly Mock<IApontamentoRepository> _apontamentoRepository;

        public RemoverTest() => _apontamentoRepository = new Mock<IApontamentoRepository>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idApontamento)
        {
            // Arrange
            var apontamentoMock = new ApontamentoItem { };

            _apontamentoRepository.Setup(x => x.ConsultarAsync(idApontamento)).ReturnsAsync(apontamentoMock);

            var pageModel = new RemoverModel(_apontamentoRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnGetAsync(idApontamento);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnPostAsync(int idApontamento)
        {
            // Arrange
            var apontamentoMock = new ApontamentoItem { IdApontamento = idApontamento };

            _apontamentoRepository.Setup(x => x.RemoverAsync(apontamentoMock));

            var pageModel = new RemoverModel(_apontamentoRepository.Object)
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
