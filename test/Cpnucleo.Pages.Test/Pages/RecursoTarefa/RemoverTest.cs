using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.RecursoTarefa;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.RecursoTarefa
{
    public class RemoverTest
    {
        private readonly Mock<IRecursoTarefaRepository> _recursoTarefaRepository;
        private readonly Mock<ITarefaRepository> _tarefaRepository;

        public RemoverTest()
        {
            _recursoTarefaRepository = new Mock<IRecursoTarefaRepository>();
            _tarefaRepository = new Mock<ITarefaRepository>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idRecursoTarefa)
        {
            // Arrange
            var recursoTarefaMock = new RecursoTarefaItem { };

            _recursoTarefaRepository.Setup(x => x.ConsultarAsync(idRecursoTarefa)).ReturnsAsync(recursoTarefaMock);

            var removerModel = new RemoverModel(_recursoTarefaRepository.Object, _tarefaRepository.Object);

            // Act
            var actionResult = await removerModel.OnGetAsync(idRecursoTarefa);

            // Assert
            Assert.NotNull(actionResult);
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnPostAsync(int idTarefa)
        {
            // Arrange
            var recursoTarefaMock = new RecursoTarefaItem { };

            _recursoTarefaRepository.Setup(x => x.RemoverAsync(recursoTarefaMock));

            var removerModel = new RemoverModel(_recursoTarefaRepository.Object, _tarefaRepository.Object);

            // Act
            var actionResult = await removerModel.OnPostAsync(idTarefa);

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
