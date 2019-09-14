using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.ImpedimentoTarefa;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.ImpedimentoTarefa
{
    public class RemoverTest
    {
        private readonly Mock<IImpedimentoTarefaRepository> _impedimentoTarefaRepository;

        public RemoverTest() => _impedimentoTarefaRepository = new Mock<IImpedimentoTarefaRepository>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGetAsync(int idImpedimentoTarefa)
        {
            // Arrange
            var impedimentoTarefaMock = new ImpedimentoTarefaModel { };

            _impedimentoTarefaRepository.Setup(x => x.ConsultarAsync(idImpedimentoTarefa)).ReturnsAsync(impedimentoTarefaMock);

            var pageModel = new RemoverModel(_impedimentoTarefaRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGetAsync(idImpedimentoTarefa))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnPostAsync(int idTarefa)
        {
            // Arrange
            var impedimentoTarefaMock = new ImpedimentoTarefaModel { };

            _impedimentoTarefaRepository.Setup(x => x.RemoverAsync(impedimentoTarefaMock));

            var pageModel = new RemoverModel(_impedimentoTarefaRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnPostAsync(idTarefa))

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
