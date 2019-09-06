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
    public class AlterarTest
    {
        private readonly Mock<IImpedimentoTarefaRepository> _impedimentoTarefaRepository;
        private readonly Mock<IRepository<ImpedimentoModel>> _impedimentoRepository;

        public AlterarTest()
        {
            _impedimentoTarefaRepository = new Mock<IImpedimentoTarefaRepository>();
            _impedimentoRepository = new Mock<IRepository<ImpedimentoModel>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idImpedimentoTarefa)
        {
            // Arrange
            var impedimentoTarefaMock = new ImpedimentoTarefaModel { };
            var listaMock = new List<ImpedimentoModel> { };

            _impedimentoTarefaRepository.Setup(x => x.ConsultarAsync(idImpedimentoTarefa)).ReturnsAsync(impedimentoTarefaMock);
            _impedimentoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var pageModel = new AlterarModel(_impedimentoTarefaRepository.Object, _impedimentoRepository.Object);

            // Act
            var result = await pageModel.OnGetAsync(idImpedimentoTarefa);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        public async Task Test_OnPostAsync(int idImpedimentoTarefa, int idImpedimento, int idTarefa)
        {
            // Arrange
            var impedimentoTarefaMock = new ImpedimentoTarefaModel { IdImpedimentoTarefa = idImpedimentoTarefa, IdImpedimento = idImpedimento, IdTarefa = idTarefa };
            var listaMock = new List<ImpedimentoModel> { };

            _impedimentoTarefaRepository.Setup(x => x.AlterarAsync(impedimentoTarefaMock));
            _impedimentoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var pageModel = new AlterarModel(_impedimentoTarefaRepository.Object, _impedimentoRepository.Object);

            // Act
            var result = await pageModel.OnPostAsync(idTarefa);

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}
