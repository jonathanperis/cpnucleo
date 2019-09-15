using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Tarefa;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Tarefa
{
    public class AlterarTest
    {
        private readonly Mock<ITarefaAppService> _tarefaAppService;
        private readonly Mock<IAppService<ProjetoViewModel>> _projetoAppService;
        private readonly Mock<IAppService<SistemaViewModel>> _sistemaAppService;
        private readonly Mock<IWorkflowAppService> _workflowAppService;
        private readonly Mock<IAppService<TipoTarefaViewModel>> _tipoTarefaAppService;

        public AlterarTest()
        {
            _tarefaAppService = new Mock<ITarefaAppService>();
            _projetoAppService = new Mock<IAppService<ProjetoViewModel>>();
            _sistemaAppService = new Mock<IAppService<SistemaViewModel>>();
            _workflowAppService = new Mock<IWorkflowAppService>();
            _tipoTarefaAppService = new Mock<IAppService<TipoTarefaViewModel>>();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            TarefaViewModel tarefaMock = new TarefaViewModel { };
            List<ProjetoViewModel> listaProjetosMock = new List<ProjetoViewModel> { };
            List<SistemaViewModel> listaSistemasMock = new List<SistemaViewModel> { };
            List<WorkflowViewModel> listaWorkflowMock = new List<WorkflowViewModel> { };
            List<TipoTarefaViewModel> listaTipoTarefaMock = new List<TipoTarefaViewModel> { };

            _tarefaAppService.Setup(x => x.Consultar(id)).Returns(tarefaMock);
            _projetoAppService.Setup(x => x.Listar()).Returns(listaProjetosMock);
            _sistemaAppService.Setup(x => x.Listar()).Returns(listaSistemasMock);
            _workflowAppService.Setup(x => x.Listar()).Returns(listaWorkflowMock);
            _tipoTarefaAppService.Setup(x => x.Listar()).Returns(listaTipoTarefaMock);

            AlterarModel pageModel = new AlterarModel(_tarefaAppService.Object, _projetoAppService.Object, _sistemaAppService.Object, _workflowAppService.Object, _tipoTarefaAppService.Object);
            PageModelTester<AlterarModel> pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1, "Tarefa de Teste", 1)]
        public void Test_OnPost(Guid id, string nome, Guid idProjeto)
        {
            // Arrange
            TarefaViewModel tarefaMock = new TarefaViewModel { Id = id, Nome = nome, IdProjeto = idProjeto };
            List<ProjetoViewModel> listaProjetosMock = new List<ProjetoViewModel> { };
            List<SistemaViewModel> listaSistemasMock = new List<SistemaViewModel> { };
            List<WorkflowViewModel> listaWorkflowMock = new List<WorkflowViewModel> { };
            List<TipoTarefaViewModel> listaTipoTarefaMock = new List<TipoTarefaViewModel> { };

            _tarefaAppService.Setup(x => x.Alterar(tarefaMock));
            _projetoAppService.Setup(x => x.Listar()).Returns(listaProjetosMock);
            _sistemaAppService.Setup(x => x.Listar()).Returns(listaSistemasMock);
            _workflowAppService.Setup(x => x.Listar()).Returns(listaWorkflowMock);
            _tipoTarefaAppService.Setup(x => x.Listar()).Returns(listaTipoTarefaMock);

            AlterarModel pageModel = new AlterarModel(_tarefaAppService.Object, _projetoAppService.Object, _sistemaAppService.Object, _workflowAppService.Object, _tipoTarefaAppService.Object);
            PageModelTester<AlterarModel> pageTester = new PageModelTester<AlterarModel>(pageModel);

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
