using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.RecursoTarefa;
using Cpnucleo.Application.Interfaces;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System.Collections.Generic;
using Xunit;
using System;

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

        [Theory]
        [InlineData(1, 1)]
        public void Test_OnGet(Guid idRecursoTarefa, Guid idProjeto)
        {
            // Arrange
            var recursoTarefaMock = new RecursoTarefaViewModel { Tarefa = new TarefaViewModel { } };
            var listaRecursoProjetoMock = new List<RecursoProjetoViewModel> { };

            _recursoTarefaAppService.Setup(x => x.Consultar(idRecursoTarefa)).Returns(recursoTarefaMock);
            _recursoProjetoAppService.Setup(x => x.ListarPorProjeto(idProjeto)).Returns(listaRecursoProjetoMock);

            var pageModel = new AlterarModel(_recursoTarefaAppService.Object, _recursoProjetoAppService.Object, _tarefaAppService.Object);

            var pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(idRecursoTarefa))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1, 56)]
        public void Test_OnPost(Guid idTarefa, int percentualTarefa)
        {
            // Arrange
            var listaMock = new List<RecursoProjetoViewModel> { };
            var tarefaMock = new TarefaViewModel { };
            var recursoTarefaMock = new RecursoTarefaViewModel { IdTarefa = idTarefa, PercentualTarefa = percentualTarefa, Tarefa = new TarefaModel() };

            _tarefaAppService.Setup(x => x.Consultar(idTarefa)).Returns(tarefaMock);
            _recursoProjetoAppService.Setup(x => x.ListarPorProjeto(idTarefa)).Returns(listaMock);
            _recursoTarefaAppService.Setup(x => x.Incluir(recursoTarefaMock));

            var pageModel = new AlterarModel(_recursoTarefaAppService.Object, _recursoProjetoAppService.Object, _tarefaAppService.Object);

            var pageTester = new PageModelTester<AlterarModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnPost())

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => () => x.OnPost())

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(recursoTarefaMock).ShouldReturn.NoErrors();
        }
    }
}
