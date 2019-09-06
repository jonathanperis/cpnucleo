using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Sistema;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Sistema
{
    public class AlterarTest
    {
        private readonly Mock<IRepository<SistemaModel>> _sistemaRepository;

        public AlterarTest() => _sistemaRepository = new Mock<IRepository<SistemaModel>>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGetAsync(int idSistema)
        {
            // Arrange
            var sistemaMock = new SistemaModel { };

            _sistemaRepository.Setup(x => x.ConsultarAsync(idSistema)).ReturnsAsync(sistemaMock);

            var pageModel = new AlterarModel(_sistemaRepository.Object);

            var pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGetAsync(idSistema))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1, "Sistema de Teste", "Descrição de Teste")]
        public void Test_OnPostAsync(int idSistema, string nome, string descricao)
        {
            // Arrange
            var sistemaMock = new SistemaModel { IdSistema = idSistema, Nome = nome, Descricao = descricao };

            _sistemaRepository.Setup(x => x.AlterarAsync(sistemaMock));

            var pageModel = new AlterarModel(_sistemaRepository.Object);

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
            Validation.For(sistemaMock).ShouldReturn.NoErrors();
        }
    }
}
