using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Sistema;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Sistema
{
    public class IncluirTest
    {
        private readonly Mock<IRepository<SistemaModel>> _sistemaRepository;

        public IncluirTest() => _sistemaRepository = new Mock<IRepository<SistemaModel>>();

        [Theory]
        [InlineData("Sistema de Teste", "Descrição de Teste")]
        public void Test_OnPostAsync(string nome, string descricao)
        {
            // Arrange
            var sistemaMock = new SistemaModel { Nome = nome, Descricao = descricao };

            _sistemaRepository.Setup(x => x.IncluirAsync(sistemaMock));

            var pageModel = new IncluirModel(_sistemaRepository.Object);

            var pageTester = new PageModelTester<IncluirModel>(pageModel);

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
