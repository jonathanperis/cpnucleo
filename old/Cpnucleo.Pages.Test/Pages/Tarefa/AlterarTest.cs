using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Tarefa;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Tarefa
{
    public class AlterarTest
    {
        private readonly Mock<ITarefaRepository> _tarefaRepository;
        private readonly Mock<IRepository<ProjetoModel>> _projetoRepository;
        private readonly Mock<IRepository<SistemaModel>> _sistemaRepository;
        private readonly Mock<IWorkflowRepository> _workflowRepository;
        private readonly Mock<IRepository<TipoTarefaModel>> _tipoTarefaRepository;

        public AlterarTest()
        {
            _tarefaRepository = new Mock<ITarefaRepository>();
            _projetoRepository = new Mock<IRepository<ProjetoModel>>();
            _sistemaRepository = new Mock<IRepository<SistemaModel>>();
            _workflowRepository = new Mock<IWorkflowRepository>();
            _tipoTarefaRepository = new Mock<IRepository<TipoTarefaModel>>();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnGetAsync(int idTarefa)
        {
            // Arrange
            var tarefaMock = new TarefaModel { };
            var listaProjetosMock = new List<ProjetoModel> { };
            var listaSistemasMock = new List<SistemaModel> { };
            var listaWorkflowMock = new List<WorkflowModel> { };
            var listaTipoTarefaMock = new List<TipoTarefaModel> { };

            _tarefaRepository.Setup(x => x.ConsultarAsync(idTarefa)).ReturnsAsync(tarefaMock);
            _projetoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaProjetosMock);
            _sistemaRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaSistemasMock);
            _workflowRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaWorkflowMock);
            _tipoTarefaRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaTipoTarefaMock);

            var pageModel = new AlterarModel(_tarefaRepository.Object, _projetoRepository.Object, _sistemaRepository.Object, _workflowRepository.Object, _tipoTarefaRepository.Object);

            var pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGetAsync(idTarefa))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1, "Tarefa de Teste", 1)]
        public void Test_OnPostAsync(int idTarefa, string nome, int idProjeto)
        {
            // Arrange
            var tarefaMock = new TarefaModel { IdTarefa = idTarefa, Nome = nome, IdProjeto = idProjeto };
            var listaProjetosMock = new List<ProjetoModel> { };
            var listaSistemasMock = new List<SistemaModel> { };
            var listaWorkflowMock = new List<WorkflowModel> { };
            var listaTipoTarefaMock = new List<TipoTarefaModel> { };

            _tarefaRepository.Setup(x => x.AlterarAsync(tarefaMock));
            _projetoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaProjetosMock);
            _sistemaRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaSistemasMock);
            _workflowRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaWorkflowMock);
            _tipoTarefaRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaTipoTarefaMock);

            var pageModel = new AlterarModel(_tarefaRepository.Object, _projetoRepository.Object, _sistemaRepository.Object, _workflowRepository.Object, _tipoTarefaRepository.Object);

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
            Validation.For(tarefaMock).ShouldReturn.NoErrors();
        }
    }
}
