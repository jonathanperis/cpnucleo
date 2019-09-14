using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.RecursoTarefa;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.RecursoTarefa
{
    public class IncluirTest
    {
        private readonly Mock<IRecursoTarefaRepository> _recursoTarefaRepository;
        private readonly Mock<IRecursoProjetoRepository> _recursoProjetoRepository;
        private readonly Mock<ITarefaRepository> _tarefaRepository;

        public IncluirTest()
        {
            _recursoTarefaRepository = new Mock<IRecursoTarefaRepository>();
            _recursoProjetoRepository = new Mock<IRecursoProjetoRepository>();
            _tarefaRepository = new Mock<ITarefaRepository>();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnGetAsync(int idTarefa)
        {
            // Arrange
            var listaMock = new List<RecursoProjetoModel> { };
            var tarefaMock = new TarefaModel { };

            _tarefaRepository.Setup(x => x.ConsultarAsync(idTarefa)).ReturnsAsync(tarefaMock);
            _recursoProjetoRepository.Setup(x => x.ListarPoridProjetoAsync(idTarefa)).ReturnsAsync(listaMock);

            var pageModel = new IncluirModel(_recursoTarefaRepository.Object, _recursoProjetoRepository.Object, _tarefaRepository.Object);

            var pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGetAsync(idTarefa))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1, 56)]
        public void Test_OnPostAsync(int idTarefa, int percentualTarefa)
        {
            // Arrange
            var listaMock = new List<RecursoProjetoModel> { };
            var tarefaMock = new TarefaModel { };
            var recursoTarefaMock = new RecursoTarefaModel { IdTarefa = idTarefa, PercentualTarefa = percentualTarefa, Tarefa = new TarefaModel() };

            _tarefaRepository.Setup(x => x.ConsultarAsync(idTarefa)).ReturnsAsync(tarefaMock);
            _recursoProjetoRepository.Setup(x => x.ListarPoridProjetoAsync(idTarefa)).ReturnsAsync(listaMock);
            _recursoTarefaRepository.Setup(x => x.IncluirAsync(recursoTarefaMock));

            var pageModel = new IncluirModel(_recursoTarefaRepository.Object, _recursoProjetoRepository.Object, _tarefaRepository.Object);

            var pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnPostAsync(idTarefa))

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => () => x.OnPostAsync(idTarefa))

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(recursoTarefaMock).ShouldReturn.NoErrors();
        }
    }
}
