using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Tarefa;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Tarefa
{
    public class RemoverTest
    {
        private readonly Mock<ITarefaRepository> _tarefaRepository;

        public RemoverTest() => _tarefaRepository = new Mock<ITarefaRepository>();

        [Theory]
        [InlineData(1)]
        public void Test_OnGetAsync(int idTarefa)
        {
            // Arrange
            var tarefaMock = new TarefaModel { };

            _tarefaRepository.Setup(x => x.ConsultarAsync(idTarefa)).ReturnsAsync(tarefaMock);

            var pageModel = new RemoverModel(_tarefaRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGetAsync(idTarefa))

                // Assert
                .TestPage();
        }

        [Fact]
        public void Test_OnPostAsync()
        {
            // Arrange
            var tarefaMock = new TarefaModel { };

            _tarefaRepository.Setup(x => x.RemoverAsync(tarefaMock));

            var pageModel = new RemoverModel(_tarefaRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPostAsync)

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
