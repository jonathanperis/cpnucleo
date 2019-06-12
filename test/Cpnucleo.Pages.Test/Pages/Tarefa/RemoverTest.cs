using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Tarefa;
using Cpnucleo.Pages.Repository;
using Moq;
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
            var tarefaMock = new TarefaItem { };

            _tarefaRepository.Setup(x => x.ConsultarAsync(idTarefa)).ReturnsAsync(tarefaMock);

            var RemoverModel = new RemoverModel(_tarefaRepository.Object);

            // Act
            var actionResult = await RemoverModel.OnGetAsync(idTarefa);

            // Assert
            Assert.NotNull(actionResult);
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnPostAsync(int idTarefa)
        {
            // Arrange
            var tarefaMock = new TarefaItem { IdTarefa = idTarefa };

            _tarefaRepository.Setup(x => x.RemoverAsync(tarefaMock));

            var removerModel = new RemoverModel(_tarefaRepository.Object);

            // Act
            var actionResult = await removerModel.OnPostAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
