using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Projeto;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Projeto
{
    public class IncluirTest
    {
        private readonly Mock<IRepository<ProjetoItem>> _projetoRepository;
        private readonly Mock<IRepository<SistemaItem>> _sistemaRepository;

        public IncluirTest()
        {
            _projetoRepository = new Mock<IRepository<ProjetoItem>>();
            _sistemaRepository = new Mock<IRepository<SistemaItem>>();
        }

        [Theory]
        [InlineData("Projeto de Teste", 1)]
        public async Task Test_OnPostAsync(string nome, int idSistema)
        {
            // Arrange
            var projetoMock = new ProjetoItem { Nome = nome, IdSistema = idSistema };
            var listaMock = new List<SistemaItem> { };

            _projetoRepository.Setup(x => x.IncluirAsync(projetoMock));
            _sistemaRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var pageModel = new IncluirModel(_projetoRepository.Object, _sistemaRepository.Object)
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
