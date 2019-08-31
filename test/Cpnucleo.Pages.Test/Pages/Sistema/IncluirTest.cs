using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Sistema;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Sistema
{
    public class IncluirTest
    {
        private readonly Mock<IRepository<SistemaItem>> _sistemaRepository;

        public IncluirTest() => _sistemaRepository = new Mock<IRepository<SistemaItem>>();

        [Theory]
        [InlineData("Sistema de Teste", "Descrição de Teste")]
        public async Task Test_OnPostAsync(string nome, string descricao)
        {
            // Arrange
            var sistemaMock = new SistemaItem { Nome = nome };

            _sistemaRepository.Setup(x => x.IncluirAsync(sistemaMock));

            var pageModel = new IncluirModel(_sistemaRepository.Object)
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
