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
        [InlineData(1, 1)]
        public void Test_OnGet(Guid idTarefa, Guid idProjeto)
        {
            // Arrange
            List<RecursoProjetoViewModel> listaMock = new List<RecursoProjetoViewModel> { };
            TarefaViewModel tarefaMock = new TarefaViewModel { };

            _tarefaAppService.Setup(x => x.Consultar(idTarefa)).Returns(tarefaMock);
            _recursoProjetoAppService.Setup(x => x.ListarPorProjeto(idProjeto)).Returns(listaMock);

            IncluirModel pageModel = new IncluirModel(_recursoTarefaAppService.Object, _recursoProjetoAppService.Object, _tarefaAppService.Object);
            PageModelTester<IncluirModel> pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(idTarefa))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1, 1, 56)]
        public void Test_OnPost(Guid idTarefa, Guid idProjeto, int percentualTarefa)
        {
            // Arrange
            List<RecursoProjetoViewModel> listaMock = new List<RecursoProjetoViewModel> { };
            TarefaViewModel tarefaMock = new TarefaViewModel { };
            RecursoTarefaViewModel recursoTarefaMock = new RecursoTarefaViewModel { IdTarefa = idTarefa, PercentualTarefa = percentualTarefa, Tarefa = new TarefaViewModel() };

            _tarefaAppService.Setup(x => x.Consultar(idTarefa)).Returns(tarefaMock);
            _recursoProjetoAppService.Setup(x => x.ListarPorProjeto(idProjeto)).Returns(listaMock);
            _recursoTarefaAppService.Setup(x => x.Incluir(recursoTarefaMock));

            IncluirModel pageModel = new IncluirModel(_recursoTarefaAppService.Object, _recursoProjetoAppService.Object, _tarefaAppService.Object);
            PageModelTester<IncluirModel> pageTester = new PageModelTester<IncluirModel>(pageModel);

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
