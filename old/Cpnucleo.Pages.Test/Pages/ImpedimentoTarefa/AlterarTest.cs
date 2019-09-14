using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.ImpedimentoTarefa;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System.Collections.Generic;
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
        public void Test_OnGetAsync(int idImpedimentoTarefa)
        {
            // Arrange
            var impedimentoTarefaMock = new ImpedimentoTarefaModel { };
            var listaMock = new List<ImpedimentoModel> { };

            _impedimentoTarefaRepository.Setup(x => x.ConsultarAsync(idImpedimentoTarefa)).ReturnsAsync(impedimentoTarefaMock);
            _impedimentoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var pageModel = new AlterarModel(_impedimentoTarefaRepository.Object, _impedimentoRepository.Object);

            var pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGetAsync(idImpedimentoTarefa))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1, 1, 1, "Descrição do Impedimento")]
        public void Test_OnPostAsync(int idImpedimentoTarefa, int idImpedimento, int idTarefa, string descricao)
        {
            // Arrange
            var impedimentoTarefaMock = new ImpedimentoTarefaModel { IdImpedimentoTarefa = idImpedimentoTarefa, IdImpedimento = idImpedimento, IdTarefa = idTarefa, Descricao = descricao };
            var listaMock = new List<ImpedimentoModel> { };

            _impedimentoTarefaRepository.Setup(x => x.AlterarAsync(impedimentoTarefaMock));
            _impedimentoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var pageModel = new AlterarModel(_impedimentoTarefaRepository.Object, _impedimentoRepository.Object);

            var pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnPostAsync(idImpedimentoTarefa))

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => () => x.OnPostAsync(idImpedimentoTarefa))

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(impedimentoTarefaMock).ShouldReturn.NoErrors();
        }
    }
}
