using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.RecursoTarefa;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.RecursoTarefa
{
    public class AlterarTest
    {
        private readonly Mock<IRecursoTarefaRepository> _recursoTarefaRepository;
        private readonly Mock<IRecursoProjetoRepository> _recursoProjetoRepository;
        private readonly Mock<ITarefaRepository> _tarefaRepository;

        public AlterarTest()
        {
            _recursoTarefaRepository = new Mock<IRecursoTarefaRepository>();
            _recursoProjetoRepository = new Mock<IRecursoProjetoRepository>();
            _tarefaRepository = new Mock<ITarefaRepository>();
        }

        [Theory]
        [InlineData(1, 1)]
        public async Task Test_OnGetAsync(int idRecursoTarefa, int idProjeto)
        {
            // Arrange
            var recursoTarefaMock = new RecursoTarefaItem { Tarefa = new TarefaItem { } };
            var listaRecursoProjetoMock = new List<RecursoProjetoItem> { };

            _recursoTarefaRepository.Setup(x => x.ConsultarAsync(idRecursoTarefa)).ReturnsAsync(recursoTarefaMock);
            _recursoProjetoRepository.Setup(x => x.ListarPoridProjetoAsync(idProjeto)).ReturnsAsync(listaRecursoProjetoMock);

            var pageModel = new AlterarModel(_recursoTarefaRepository.Object, _recursoProjetoRepository.Object, _tarefaRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnGetAsync(idRecursoTarefa);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnPostAsync(int idTarefa)
        {
            // Arrange
            var recursoTarefaMock = new RecursoTarefaItem { };

            _recursoTarefaRepository.Setup(x => x.AlterarAsync(recursoTarefaMock));

            var pageModel = new AlterarModel(_recursoTarefaRepository.Object, _recursoProjetoRepository.Object, _tarefaRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnPostAsync(idTarefa);

            // Assert
            Assert.IsType<PageResult>(result);
        }
    }
}
