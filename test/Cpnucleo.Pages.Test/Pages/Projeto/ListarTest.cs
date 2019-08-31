using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Projeto;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Projeto
{
    public class ListarTest
    {
        private readonly Mock<IRepository<ProjetoItem>> _projetoRepository;

        public ListarTest() => _projetoRepository = new Mock<IRepository<ProjetoItem>>();

        [Fact]
        public async Task Test_OnGetAsync()
        {
            // Arrange
            var listaMock = new List<ProjetoItem> { };

            _projetoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var pageModel = new ListarModel(_projetoRepository.Object)
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
