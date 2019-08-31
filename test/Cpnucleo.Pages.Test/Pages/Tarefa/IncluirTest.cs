using Cpnucleo.Pages.Authentication;
using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Tarefa;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Tarefa
{
    public class IncluirTest
    {
        private readonly Mock<IClaimsManager> _claimsManager;

        private readonly Mock<ITarefaRepository> _tarefaRepository;
        private readonly Mock<IRepository<ProjetoItem>> _projetoRepository;
        private readonly Mock<IRepository<SistemaItem>> _sistemaRepository;
        private readonly Mock<IWorkflowRepository> _workflowRepository;
        private readonly Mock<IRepository<TipoTarefaItem>> _tipoTarefaRepository;

        public IncluirTest()
        {
            _claimsManager = new Mock<IClaimsManager>();

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

            var pageModel = new IncluirModel(_claimsManager.Object, _tarefaRepository.Object, _projetoRepository.Object, _sistemaRepository.Object, _workflowRepository.Object, _tipoTarefaRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData("Tarefa de Teste", 1)]
        public async Task Test_OnPostAsync(string nome, int idProjeto)
        {
            // Arrange
            var idRecursoMock = "1";
            var principalMock = new ClaimsPrincipal { };

            var tarefaMock = new TarefaItem { Nome = nome, IdProjeto = idProjeto };

            _claimsManager.Setup(x => x.ReadClaimsPrincipal(principalMock, ClaimTypes.PrimarySid)).Returns(idRecursoMock);

            _tarefaRepository.Setup(x => x.IncluirAsync(tarefaMock));

            var pageModel = new IncluirModel(_claimsManager.Object, _tarefaRepository.Object, _projetoRepository.Object, _sistemaRepository.Object, _workflowRepository.Object, _tipoTarefaRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}
