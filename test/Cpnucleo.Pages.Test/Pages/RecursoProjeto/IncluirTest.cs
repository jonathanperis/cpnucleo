using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.RecursoProjeto;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.RecursoProjeto
{
    public class IncluirTest
    {
        private readonly Mock<IRecursoProjetoRepository> _recursoProjetoRepository;
        private readonly Mock<IRecursoRepository> _recursoRepository;
        private readonly Mock<IRepository<ProjetoItem>> _projetoRepository;

        public IncluirTest()
        {
            _recursoProjetoRepository = new Mock<IRecursoProjetoRepository>();
            _recursoRepository = new Mock<IRecursoRepository>();
            _projetoRepository = new Mock<IRepository<ProjetoItem>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idProjeto)
        {
            // Arrange
            var projetoMock = new ProjetoItem { };
            var listaMock = new List<RecursoItem> { };

            _projetoRepository.Setup(x => x.ConsultarAsync(idProjeto)).ReturnsAsync(projetoMock);
            _recursoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var pageModel = new IncluirModel(_recursoProjetoRepository.Object, _recursoRepository.Object, _projetoRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnGetAsync(idProjeto);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnPostAsync(int idProjeto)
        {
            // Arrange
            var projetoMock = new ProjetoItem { };
            var listaMock = new List<RecursoItem> { };
            var recursoProjetoMock = new RecursoProjetoItem { };

            _projetoRepository.Setup(x => x.ConsultarAsync(idProjeto)).ReturnsAsync(projetoMock);
            _recursoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);
            _recursoProjetoRepository.Setup(x => x.IncluirAsync(recursoProjetoMock));

            var pageModel = new IncluirModel(_recursoProjetoRepository.Object, _recursoRepository.Object, _projetoRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnPostAsync(idProjeto);

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}
