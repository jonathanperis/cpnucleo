using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Impedimento;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Impedimento
{
    public class ListarTest
    {
        private readonly Mock<IRepository<ImpedimentoItem>> _impedimentoRepository;

        public ListarTest() => _impedimentoRepository = new Mock<IRepository<ImpedimentoItem>>();

        [Fact]
        public async Task Test_OnGetAsync()
        {
            // Arrange
            var listaMock = new List<ImpedimentoItem> { };

            _impedimentoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var pageModel = new ListarModel(_impedimentoRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }
    }
}
