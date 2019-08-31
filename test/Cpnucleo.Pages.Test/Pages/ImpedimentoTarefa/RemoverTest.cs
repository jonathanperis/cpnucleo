using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.ImpedimentoTarefa;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.ImpedimentoTarefa
{
    public class RemoverTest
    {
        private readonly Mock<IImpedimentoTarefaRepository> _impedimentoTarefaRepository;

        public RemoverTest() => _impedimentoTarefaRepository = new Mock<IImpedimentoTarefaRepository>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idImpedimentoTarefa)
        {
            // Arrange
            var impedimentoTarefaMock = new ImpedimentoTarefaItem { };

            _impedimentoTarefaRepository.Setup(x => x.ConsultarAsync(idImpedimentoTarefa)).ReturnsAsync(impedimentoTarefaMock);

            var pageModel = new RemoverModel(_impedimentoTarefaRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnGetAsync(idImpedimentoTarefa);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(1, 1)]
        public async Task Test_OnPostAsync(int idImpedimentoTarefa, int idTarefa)
        {
            // Arrange
            var impedimentoTarefaMock = new ImpedimentoTarefaItem { IdImpedimentoTarefa = idImpedimentoTarefa };

            _impedimentoTarefaRepository.Setup(x => x.RemoverAsync(impedimentoTarefaMock));

            var pageModel = new RemoverModel(_impedimentoTarefaRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnPostAsync(idTarefa);

            // Assert
            Assert.IsType<PageResult>(result);
        }
    }
}
