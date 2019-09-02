using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.ImpedimentoTarefa;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.ImpedimentoTarefa
{
    public class IncluirTest
    {
        private readonly Mock<IImpedimentoTarefaRepository> _impedimentoTarefaRepository;
        private readonly Mock<IRepository<ImpedimentoItem>> _impedimentoRepository;
        private readonly Mock<ITarefaRepository> _tarefaRepository;

        public IncluirTest()
        {
            _impedimentoTarefaRepository = new Mock<IImpedimentoTarefaRepository>();
            _impedimentoRepository = new Mock<IRepository<ImpedimentoItem>>();
            _tarefaRepository = new Mock<ITarefaRepository>();
        }

        [Theory]
        [InlineData(1, 1)]
        public async Task Test_OnGetAsync(int idImpedimentoTarefa, int idTarefa)
        {
            // Arrange
            var impedimentoTarefaMock = new ImpedimentoTarefaItem { };
            var listaMock = new List<ImpedimentoItem> { };
            var tarefaMock = new TarefaItem { };

            _impedimentoTarefaRepository.Setup(x => x.ConsultarAsync(idImpedimentoTarefa)).ReturnsAsync(impedimentoTarefaMock);
            _impedimentoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);
            _tarefaRepository.Setup(x => x.ConsultarAsync(idTarefa)).ReturnsAsync(tarefaMock);

            var pageModel = new IncluirModel(_impedimentoTarefaRepository.Object, _impedimentoRepository.Object, _tarefaRepository.Object);

            // Act
            var result = await pageModel.OnGetAsync(idTarefa);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(1, 1)]
        public async Task Test_OnPostAsync(int idImpedimentoTarefa, int idTarefa)
        {
            // Arrange
            var impedimentoTarefaMock = new ImpedimentoTarefaItem { IdImpedimentoTarefa = idImpedimentoTarefa, IdTarefa = idTarefa };
            var listaMock = new List<ImpedimentoItem> { };
            var tarefaMock = new TarefaItem { };

            _impedimentoTarefaRepository.Setup(x => x.IncluirAsync(impedimentoTarefaMock));
            _impedimentoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);
            _tarefaRepository.Setup(x => x.ConsultarAsync(idTarefa)).ReturnsAsync(tarefaMock);

            var pageModel = new IncluirModel(_impedimentoTarefaRepository.Object, _impedimentoRepository.Object, _tarefaRepository.Object);

            // Act
            var result = await pageModel.OnPostAsync(idTarefa);

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}
