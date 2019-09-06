using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Projeto;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Projeto
{
    public class RemoverTest
    {
        private readonly Mock<IRepository<ProjetoModel>> _projetoRepository;

        public RemoverTest() => _projetoRepository = new Mock<IRepository<ProjetoModel>>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idProjeto)
        {
            // Arrange
            var projetoMock = new ProjetoModel { };

            _projetoRepository.Setup(x => x.ConsultarAsync(idProjeto)).ReturnsAsync(projetoMock);

            var pageModel = new RemoverModel(_projetoRepository.Object);

            // Act
            var result = await pageModel.OnGetAsync(idProjeto);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(1, "Projeto de Teste", 1)]
        public void Test_OnPostAsync(int idProjeto, string nome, int idSistema)
        {
            // Arrange
            var projetoMock = new ProjetoModel { IdProjeto = idProjeto, Nome = nome, IdSistema = idSistema };

            _projetoRepository.Setup(x => x.RemoverAsync(projetoMock));

            var pageModel = new RemoverModel(_projetoRepository.Object);

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
            Validation.For(projetoMock).ShouldReturn.NoErrors();
        }
    }
}
