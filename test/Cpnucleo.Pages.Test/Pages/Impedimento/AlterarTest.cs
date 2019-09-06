using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Impedimento;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Impedimento
{
    public class AlterarTest
    {
        private readonly Mock<IRepository<ImpedimentoModel>> _impedimentoRepository;

        public AlterarTest() => _impedimentoRepository = new Mock<IRepository<ImpedimentoModel>>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGetAsync(int idImpedimento)
        {
            // Arrange
            var impedimentoMock = new ImpedimentoModel { };

            _impedimentoRepository.Setup(x => x.ConsultarAsync(idImpedimento)).ReturnsAsync(impedimentoMock);

            var pageModel = new AlterarModel(_impedimentoRepository.Object);

            var pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGetAsync(idImpedimento))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1, "Impedimento de Teste")]
        public void Test_OnPostAsync(int idImpedimento, string nome)
        {
            // Arrange
            var impedimentoMock = new ImpedimentoModel { IdImpedimento = idImpedimento, Nome = nome };

            _impedimentoRepository.Setup(x => x.AlterarAsync(impedimentoMock));

            var pageModel = new AlterarModel(_impedimentoRepository.Object);

            var pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPostAsync)

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => x.OnPostAsync)

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(impedimentoMock).ShouldReturn.NoErrors();
        }
    }
}
