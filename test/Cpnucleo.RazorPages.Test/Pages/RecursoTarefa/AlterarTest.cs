using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.RecursoTarefa;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.RecursoTarefa
{
    public class AlterarTest
    {
        private readonly Mock<IRecursoTarefaAppService> _recursoTarefaAppService;
        private readonly Mock<IRecursoProjetoAppService> _recursoProjetoAppService;
        private readonly Mock<ITarefaAppService> _tarefaAppService;

        public AlterarTest()
        {
            _recursoTarefaAppService = new Mock<IRecursoTarefaAppService>();
            _recursoProjetoAppService = new Mock<IRecursoProjetoAppService>();
            _tarefaAppService = new Mock<ITarefaAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid id = new Guid();
            Guid idProjeto = new Guid();

            RecursoTarefaViewModel recursoTarefaMock = new RecursoTarefaViewModel { Tarefa = new TarefaViewModel { } };
            List<RecursoProjetoViewModel> listaRecursoProjetoMock = new List<RecursoProjetoViewModel> { };

            _recursoTarefaAppService.Setup(x => x.Consultar(id)).Returns(recursoTarefaMock);
            _recursoProjetoAppService.Setup(x => x.ListarPorProjeto(idProjeto)).Returns(listaRecursoProjetoMock);

            AlterarModel pageModel = new AlterarModel(_recursoTarefaAppService.Object, _recursoProjetoAppService.Object, _tarefaAppService.Object);
            PageModelTester<AlterarModel> pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(56)]
        public void Test_OnPost(int percentualTarefa)
        {
            // Arrange
            Guid idTarefa = new Guid();
            Guid idProjeto = new Guid();

            List<RecursoProjetoViewModel> listaMock = new List<RecursoProjetoViewModel> { };
            TarefaViewModel tarefaMock = new TarefaViewModel { };
            RecursoTarefaViewModel recursoTarefaMock = new RecursoTarefaViewModel { IdTarefa = idTarefa, PercentualTarefa = percentualTarefa, Tarefa = new TarefaViewModel() };

            _tarefaAppService.Setup(x => x.Consultar(idTarefa)).Returns(tarefaMock);
            _recursoProjetoAppService.Setup(x => x.ListarPorProjeto(idProjeto)).Returns(listaMock);
            _recursoTarefaAppService.Setup(x => x.Incluir(recursoTarefaMock));

            AlterarModel pageModel = new AlterarModel(_recursoTarefaAppService.Object, _recursoProjetoAppService.Object, _tarefaAppService.Object)
            {
                RecursoTarefa = new RecursoTarefaViewModel { IdTarefa = idTarefa },
                Tarefa = new TarefaViewModel { IdProjeto = idProjeto }
            };

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
            Validation.For(recursoTarefaMock).ShouldReturn.NoErrors();
        }
    }
}
