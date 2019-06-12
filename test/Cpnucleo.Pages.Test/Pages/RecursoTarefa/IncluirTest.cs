using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.RecursoTarefa;
using Cpnucleo.Pages.Repository;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.RecursoTarefa
{
    public class IncluirTest
    {
        private readonly Mock<IRecursoTarefaRepository> _recursoTarefaRepository;
        private readonly Mock<IRecursoProjetoRepository> _recursoProjetoRepository;
        private readonly Mock<ITarefaRepository> _tarefaRepository;

        public IncluirTest()
        {
            _recursoTarefaRepository = new Mock<IRecursoTarefaRepository>();
            _recursoProjetoRepository = new Mock<IRecursoProjetoRepository>();
            _tarefaRepository = new Mock<ITarefaRepository>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idTarefa)
        {
            // Arrange
            var listaMock = new List<RecursoTarefaItem> { };

            _recursoProjetoRepository.Setup(x => x.ListarPoridProjetoAsync());
            _sistemaRepository.Setup(x => x.ListarPoridTarefaAsync(idTarefa)).ReturnsAsync(listaMock);

            var incluirModel = new IncluirModel(_recursoTarefaRepository.Object);

            // Act
            var actionResult = await incluirModel.OnGetAsync(idTarefa);

            // Assert
            Assert.NotNull(actionResult);
        }

        [Theory]
        [InlineData("RecursoTarefa de Teste", "Descrição de Teste")]
        public async Task Test_OnPostAsync(string nome, string descricao)
        {
            // Arrange
            var recursoTarefaMock = new RecursoTarefaItem { Nome = nome, Descricao = descricao };

            _recursoTarefaRepository.Setup(x => x.IncluirAsync(recursoTarefaMock));

            var incluirModel = new IncluirModel(_recursoTarefaRepository.Object);

            // Act
            var actionResult = await incluirModel.OnPostAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
