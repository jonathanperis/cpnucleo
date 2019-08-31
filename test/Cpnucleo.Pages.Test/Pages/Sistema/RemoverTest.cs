using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Sistema;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Sistema
{
    public class RemoverTest
    {
        private readonly Mock<IRepository<SistemaItem>> _sistemaRepository;

        public RemoverTest() => _sistemaRepository = new Mock<IRepository<SistemaItem>>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idSistema)
        {
            // Arrange
            var sistemaMock = new SistemaItem { };

            _sistemaRepository.Setup(x => x.ConsultarAsync(idSistema)).ReturnsAsync(sistemaMock);

            var pageModel = new RemoverModel(_sistemaRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnGetAsync(idSistema);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnPostAsync(int idSistema)
        {
            // Arrange
            var sistemaMock = new SistemaItem { IdSistema = idSistema };

            _sistemaRepository.Setup(x => x.RemoverAsync(sistemaMock));

            var pageModel = new RemoverModel(_sistemaRepository.Object)
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
