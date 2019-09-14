using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.ImpedimentoTarefa;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.ImpedimentoTarefa
{
    public class ListarTest
    {
        private readonly Mock<IImpedimentoTarefaRepository> _impedimentoTarefaRepository;

        public ListarTest() => _impedimentoTarefaRepository = new Mock<IImpedimentoTarefaRepository>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGetAsync(int idTarefa)
        {
            // Arrange
            var listaMock = new List<ImpedimentoTarefaModel> { };

            _impedimentoTarefaRepository.Setup(x => x.ListarPoridTarefaAsync(idTarefa)).ReturnsAsync(listaMock);

            var pageModel = new ListarModel(_impedimentoTarefaRepository.Object)
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