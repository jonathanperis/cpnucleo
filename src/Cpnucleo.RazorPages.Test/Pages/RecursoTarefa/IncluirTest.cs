using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
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

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid idTarefa = Guid.NewGuid();
            Guid idProjeto = Guid.NewGuid();

            List<RecursoProjetoViewModel> listaMock = new List<RecursoProjetoViewModel> { };
            TarefaViewModel tarefaMock = new TarefaViewModel { };

            IncluirModel pageModel = new IncluirModel(_recursoTarefaAppService.Object, _recursoProjetoAppService.Object, _tarefaAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _tarefaAppService.Setup(x => x.Consultar(idTarefa)).Returns(tarefaMock);
            _recursoProjetoAppService.Setup(x => x.ListarPorProjeto(idProjeto)).Returns(listaMock);

            PageModelTester<IncluirModel> pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(idTarefa))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(56)]
        public void Test_OnPost(int percentualTarefa)
        {
            // Arrange
            Guid idTarefa = Guid.NewGuid();
            Guid idProjeto = Guid.NewGuid();

            List<RecursoProjetoViewModel> listaMock = new List<RecursoProjetoViewModel> { };
            TarefaViewModel tarefaMock = new TarefaViewModel { };
            RecursoTarefaViewModel recursoTarefaMock = new RecursoTarefaViewModel { IdTarefa = idTarefa, PercentualTarefa = percentualTarefa, Tarefa = new TarefaViewModel() };

            IncluirModel pageModel = new IncluirModel(_recursoTarefaAppService.Object, _recursoProjetoAppService.Object, _tarefaAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _tarefaAppService.Setup(x => x.Consultar(idTarefa)).Returns(tarefaMock);
            _recursoProjetoAppService.Setup(x => x.ListarPorProjeto(idProjeto)).Returns(listaMock);
            _recursoTarefaAppService.Setup(x => x.Incluir(recursoTarefaMock));

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
