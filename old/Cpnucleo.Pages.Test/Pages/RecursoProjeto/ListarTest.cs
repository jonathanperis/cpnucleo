using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.RecursoProjeto;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.RecursoProjeto
{
    public class ListarTest
    {
        private readonly Mock<IRecursoProjetoRepository> _recursoProjetoRepository;

        public ListarTest() => _recursoProjetoRepository = new Mock<IRecursoProjetoRepository>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGetAsync(int idProjeto)
        {
            // Arrange
            var listaMock = new List<RecursoProjetoModel> { };

            _recursoProjetoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var pageModel = new ListarModel(_recursoProjetoRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            var pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGetAsync(idProjeto))

                // Assert
                .TestPage();
        }
    }
}