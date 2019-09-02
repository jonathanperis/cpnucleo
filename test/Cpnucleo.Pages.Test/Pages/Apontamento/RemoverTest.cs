using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Apontamento;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Apontamento
{
    public class RemoverTest
    {
        private readonly Mock<IApontamentoRepository> _apontamentoRepository;

        public RemoverTest() => _apontamentoRepository = new Mock<IApontamentoRepository>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idApontamento)
        {
            // Arrange
            var apontamentoMock = new ApontamentoItem { };

            _apontamentoRepository.Setup(x => x.ConsultarAsync(idApontamento)).ReturnsAsync(apontamentoMock);

            var pageModel = new RemoverModel(_apontamentoRepository.Object);

            // Act
            var result = await pageModel.OnGetAsync(idApontamento);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(1, "Descrição do apontamento")]
        public void Test_OnPostAsync(int idApontamento, string descricao)
        {
            // Arrange
            var apontamentoMock = new ApontamentoItem { IdApontamento = idApontamento, Descricao = descricao };

            _apontamentoRepository.Setup(x => x.RemoverAsync(apontamentoMock));

            var pageModel = new RemoverModel(_apontamentoRepository.Object);

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
                .TestRedirectToPage("/Apontamento");

            // Assert
            Validation.For(apontamentoMock).ShouldReturn.NoErrors();
        }
    }
}
