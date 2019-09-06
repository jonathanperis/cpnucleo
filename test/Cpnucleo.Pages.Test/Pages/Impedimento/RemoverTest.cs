using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Impedimento;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Impedimento
{
    public class RemoverTest
    {
        private readonly Mock<IRepository<ImpedimentoModel>> _impedimentoRepository;

        public RemoverTest() => _impedimentoRepository = new Mock<IRepository<ImpedimentoModel>>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idImpedimento)
        {
            // Arrange
            var impedimentoMock = new ImpedimentoModel { };

            _impedimentoRepository.Setup(x => x.ConsultarAsync(idImpedimento)).ReturnsAsync(impedimentoMock);

            var pageModel = new RemoverModel(_impedimentoRepository.Object);

            // Act
            var result = await pageModel.OnGetAsync(idImpedimento);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(1, "Impedimento de Teste")]
        public void Test_OnPostAsync(int idImpedimento, string nome)
        {
            // Arrange
            var impedimentoMock = new ImpedimentoModel { IdImpedimento = idImpedimento, Nome = nome };

            _impedimentoRepository.Setup(x => x.RemoverAsync(impedimentoMock));

            var pageModel = new RemoverModel(_impedimentoRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

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
