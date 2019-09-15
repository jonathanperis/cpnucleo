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
    public class IncluirTest
    {
        private readonly Mock<IRecursoTarefaAppService> _recursoTarefaAppService;
        private readonly Mock<IRecursoProjetoAppService> _recursoProjetoAppService;
        private readonly Mock<ITarefaAppService> _tarefaAppService;

        public IncluirTest()
        {
            _recursoTarefaAppService = new Mock<IRecursoTarefaAppService>();
            _recursoProjetoAppService = new Mock<IRecursoProjetoAppService>();
            _tarefaAppService = new Mock<ITarefaAppService>();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid id)
        {
            // Arrange
            var listaMock = new List<RecursoProjetoViewModel> { };
            var tarefaMock = new TarefaViewModel { };

            _tarefaAppService.Setup(x => x.Consultar(id)).Returns(tarefaMock);
            _recursoProjetoAppService.Setup(x => x.ListarPorProjeto(id)).Returns(listaMock);

            var pageModel = new IncluirModel(_recursoTarefaAppService.Object, _recursoProjetoAppService.Object, _tarefaAppService.Object);

            var pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(id))

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

            var pageModel = new IncluirModel(_recursoTarefaAppService.Object, _recursoProjetoAppService.Object, _tarefaAppService.Object);

            var pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnPost(idTarefa))

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => () => x.OnPost(idTarefa))

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(recursoTarefaMock).ShouldReturn.NoErrors();
        }
    }
}
