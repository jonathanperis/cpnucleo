using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Tarefa;
using Cpnucleo.Pages.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Tarefa
{
    public class IncluirTest
    {
        private readonly Mock<ITarefaRepository> _tarefaRepository;
        private readonly Mock<IRepository<ProjetoItem>> _projetoRepository;
        private readonly Mock<IRepository<SistemaItem>> _sistemaRepository;
        private readonly Mock<IWorkflowRepository> _workflowRepository;
        private readonly Mock<IRepository<TipoTarefaItem>> _tipoTarefaRepository;

        public IncluirTest()
        {
            _tarefaRepository = new Mock<ITarefaRepository>();
            _projetoRepository = new Mock<IRepository<ProjetoItem>>();
            _sistemaRepository = new Mock<IRepository<SistemaItem>>();
            _workflowRepository = new Mock<IWorkflowRepository>();
            _tipoTarefaRepository = new Mock<IRepository<TipoTarefaItem>>();
        }

        [Fact]
        public async Task Test_OnGetAsync()
        {
            // Arrange
            var listaProjetosMock = new List<ProjetoItem> { };
            var listaSistemasMock = new List<SistemaItem> { };
            var listaWorkflowMock = new List<WorkflowItem> { };
            var listaTipoTarefaMock = new List<TipoTarefaItem> { };

            _projetoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaProjetosMock);
            _sistemaRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaSistemasMock);
            _workflowRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaWorkflowMock);
            _tipoTarefaRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaTipoTarefaMock);

            var AlterarModel = new IncluirModel(_tarefaRepository.Object, _projetoRepository.Object, _sistemaRepository.Object, _workflowRepository.Object, _tipoTarefaRepository.Object);

            // Act
            var actionResult = await AlterarModel.OnGetAsync();

            // Assert
            Assert.NotNull(actionResult);
        }

        [Theory]
        [InlineData("Tarefa de Teste", 1, "20190610 00:00:00", "20190615 00:00:00", "Detalhe da Tarefa", 1, 1)]
        public async Task Test_OnPostAsync(string nome, int idProjeto, DateTime dataInicio, DateTime dataTermino, string detalhe, int idTipoTarefa, int idWorkflow)
        {
            // Arrange
            var tarefaMock = new TarefaItem { Nome = nome, IdProjeto = idProjeto, DataInicio = dataInicio, DataTermino = dataTermino, Detalhe = detalhe, IdTipoTarefa = idTipoTarefa, IdWorkflow = idWorkflow };

            _tarefaRepository.Setup(x => x.IncluirAsync(tarefaMock));

            var incluirModel = new IncluirModel(_tarefaRepository.Object, _projetoRepository.Object, _sistemaRepository.Object, _workflowRepository.Object, _tipoTarefaRepository.Object);

            // Act
            var actionResult = await incluirModel.OnPostAsync();

            // Assert
            Assert.NotNull(actionResult);
        }
    }
}
