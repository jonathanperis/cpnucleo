using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Impedimento;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Impedimento
{
    public class IncluirTest
    {
        private readonly Mock<IRepository<ImpedimentoItem>> _impedimentoRepository;

        public IncluirTest() => _impedimentoRepository = new Mock<IRepository<ImpedimentoItem>>();

        [Theory]
        [InlineData("Impedimento de Teste")]
        public async Task Test_OnPostAsync(string nome)
        {
            // Arrange
            var impedimentoMock = new ImpedimentoItem { Nome = nome };

            _impedimentoRepository.Setup(x => x.IncluirAsync(impedimentoMock));

            var pageModel = new IncluirModel(_impedimentoRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}
