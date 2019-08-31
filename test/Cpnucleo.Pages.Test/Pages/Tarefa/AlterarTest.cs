using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Tarefa;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Tarefa
{
    public class AlterarTest
    {
        private readonly Mock<ITarefaRepository> _tarefaRepository;
        private readonly Mock<IRepository<ProjetoItem>> _projetoRepository;
        private readonly Mock<IRepository<SistemaItem>> _sistemaRepository;
        private readonly Mock<IWorkflowRepository> _workflowRepository;
        private readonly Mock<IRepository<TipoTarefaItem>> _tipoTarefaRepository;

        public AlterarTest()
        {
            _tarefaRepository = new Mock<ITarefaRepository>();
            _projetoRepository = new Mock<IRepository<ProjetoItem>>();
            _sistemaRepository = new Mock<IRepository<SistemaItem>>();
            _workflowRepository = new Mock<IWorkflowRepository>();
            _tipoTarefaRepository = new Mock<IRepository<TipoTarefaItem>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idTarefa)
        {
            // Arrange
            var tarefaMock = new TarefaItem { };
            var listaProjetosMock = new List<ProjetoItem> { };
            var listaSistemasMock = new List<SistemaItem> { };
            var listaWorkflowMock = new List<WorkflowItem> { };
            var listaTipoTarefaMock = new List<TipoTarefaItem> { };

            _tarefaRepository.Setup(x => x.ConsultarAsync(idTarefa)).ReturnsAsync(tarefaMock);
            _projetoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaProjetosMock);
            _sistemaRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaSistemasMock);
            _workflowRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaWorkflowMock);
            _tipoTarefaRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaTipoTarefaMock);

            var pageModel = new AlterarModel(_tarefaRepository.Object, _projetoRepository.Object, _sistemaRepository.Object, _workflowRepository.Object, _tipoTarefaRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnGetAsync(idTarefa);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(1, "Tarefa de Teste", 1)]
        public async Task Test_OnPostAsync(int idTarefa, string nome, int idProjeto)
        {
            // Arrange
            var tarefaMock = new TarefaItem { IdTarefa = idTarefa, Nome = nome, IdProjeto = idProjeto };
            var listaProjetosMock = new List<ProjetoItem> { };
            var listaSistemasMock = new List<SistemaItem> { };
            var listaWorkflowMock = new List<WorkflowItem> { };
            var listaTipoTarefaMock = new List<TipoTarefaItem> { };

            _tarefaRepository.Setup(x => x.AlterarAsync(tarefaMock));
            _projetoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaProjetosMock);
            _sistemaRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaSistemasMock);
            _workflowRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaWorkflowMock);
            _tipoTarefaRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaTipoTarefaMock);

            var pageModel = new AlterarModel(_tarefaRepository.Object, _projetoRepository.Object, _sistemaRepository.Object, _workflowRepository.Object, _tipoTarefaRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }
    }
}
