using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages.Tarefa;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Tarefa
{
    public class IncluirTest
    {
        private readonly Mock<ITarefaAppService> _tarefaAppService;
        private readonly Mock<IProjetoAppService> _projetoAppService;
        private readonly Mock<ISistemaAppService> _sistemaAppService;
        private readonly Mock<IWorkflowAppService> _workflowAppService;
        private readonly Mock<ITipoTarefaAppService> _tipoTarefaAppService;

        public IncluirTest()
        {
            _tarefaAppService = new Mock<ITarefaAppService>();
            _projetoAppService = new Mock<IProjetoAppService>();
            _sistemaAppService = new Mock<ISistemaAppService>();
            _workflowAppService = new Mock<IWorkflowAppService>();
            _tipoTarefaAppService = new Mock<ITipoTarefaAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            List<ProjetoViewModel> listaProjetosMock = new List<ProjetoViewModel> { };
            List<SistemaViewModel> listaSistemasMock = new List<SistemaViewModel> { };
            List<WorkflowViewModel> listaWorkflowMock = new List<WorkflowViewModel> { };
            List<TipoTarefaViewModel> listaTipoTarefaMock = new List<TipoTarefaViewModel> { };

            _projetoAppService.Setup(x => x.Listar()).Returns(listaProjetosMock);
            _sistemaAppService.Setup(x => x.Listar()).Returns(listaSistemasMock);
            _workflowAppService.Setup(x => x.Listar()).Returns(listaWorkflowMock);
            _tipoTarefaAppService.Setup(x => x.Listar()).Returns(listaTipoTarefaMock);

            IncluirModel pageModel = new IncluirModel(_tarefaAppService.Object, _projetoAppService.Object, _sistemaAppService.Object, _workflowAppService.Object, _tipoTarefaAppService.Object);
            PageModelTester<IncluirModel> pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnGet)

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData("Tarefa de Teste")]
        public void Test_OnPost(string nome)
        {
            // Arrange
            Guid idProjeto = Guid.NewGuid();
            Guid IdWorkflow = Guid.NewGuid();
            Guid IdRecurso = Guid.NewGuid();
            Guid IdTipoTarefa = Guid.NewGuid();

            DateTime dataInicio = DateTime.Now;
            DateTime dataTermino = DateTime.Now.AddDays(5);

            TarefaViewModel tarefaMock = new TarefaViewModel
            {
                Nome = nome,
                IdProjeto = idProjeto,
                DataInicio = dataInicio,
                DataTermino = dataTermino,
                IdWorkflow = IdWorkflow,
                IdRecurso = IdRecurso,
                IdTipoTarefa = IdTipoTarefa
            };

            _tarefaAppService.Setup(x => x.Incluir(tarefaMock));

            IncluirModel pageModel = new IncluirModel(_tarefaAppService.Object, _projetoAppService.Object, _sistemaAppService.Object, _workflowAppService.Object, _tipoTarefaAppService.Object);
            PageModelTester<IncluirModel> pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(tarefaMock).ShouldReturn.NoErrors();
        }
    }
}
