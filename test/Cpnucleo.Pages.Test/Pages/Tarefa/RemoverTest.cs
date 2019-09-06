using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Tarefa;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Tarefa
{
    public class RemoverTest
    {
        private readonly Mock<ITarefaRepository> _tarefaRepository;

        public RemoverTest() => _tarefaRepository = new Mock<ITarefaRepository>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idTarefa)
        {
            // Arrange
            var tarefaMock = new TarefaModel { };

            _tarefaRepository.Setup(x => x.ConsultarAsync(idTarefa)).ReturnsAsync(tarefaMock);

            var pageModel = new RemoverModel(_tarefaRepository.Object);

            // Act
            var result = await pageModel.OnGetAsync(idTarefa);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnPostAsync(int idTarefa)
        {
            // Arrange
            var tarefaMock = new TarefaModel { IdTarefa = idTarefa };

            _tarefaRepository.Setup(x => x.RemoverAsync(tarefaMock));

            var pageModel = new RemoverModel(_tarefaRepository.Object);

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
            Validation.For(tarefaMock).ShouldReturn.NoErrors();
        }
    }
}
