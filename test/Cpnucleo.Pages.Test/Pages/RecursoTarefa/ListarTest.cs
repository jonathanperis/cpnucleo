using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.RecursoTarefa;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.RecursoTarefa
{
    public class ListarTest
    {
        private readonly Mock<IRecursoTarefaRepository> _sistemaRepository;

        public ListarTest() => _sistemaRepository = new Mock<IRecursoTarefaRepository>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGetAsync(int idTarefa)
        {
            // Arrange
            var listaMock = new List<RecursoTarefaModel> { };

            _sistemaRepository.Setup(x => x.ListarPoridTarefaAsync(idTarefa)).ReturnsAsync(listaMock);

            var pageModel = new ListarModel(_sistemaRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            var pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGetAsync(idTarefa))

                // Assert
                .TestPage();
        }
    }
}
