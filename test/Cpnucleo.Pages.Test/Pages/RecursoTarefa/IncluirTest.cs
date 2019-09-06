using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.RecursoTarefa;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task Test_OnGetAsync(int idTarefa)
        {
            // Arrange
            var listaMock = new List<RecursoProjetoModel> { };
            var tarefaMock = new TarefaModel { };

            _tarefaRepository.Setup(x => x.ConsultarAsync(idTarefa)).ReturnsAsync(tarefaMock);
            _recursoProjetoRepository.Setup(x => x.ListarPoridProjetoAsync(idTarefa)).ReturnsAsync(listaMock);

            var pageModel = new IncluirModel(_recursoTarefaRepository.Object, _recursoProjetoRepository.Object, _tarefaRepository.Object);

            // Act
            var result = await pageModel.OnGetAsync(idTarefa);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(1, "RecursoTarefa de Teste", "Descrição de Teste")]
        public async Task Test_OnPostAsync(int idTarefa, string nome, string descricao)
        {
            // Arrange
            var listaMock = new List<RecursoProjetoModel> { };
            var tarefaMock = new TarefaModel { };
            var recursoTarefaMock = new RecursoTarefaModel { Tarefa = new TarefaModel() };

            _tarefaRepository.Setup(x => x.ConsultarAsync(idTarefa)).ReturnsAsync(tarefaMock);
            _recursoProjetoRepository.Setup(x => x.ListarPoridProjetoAsync(idTarefa)).ReturnsAsync(listaMock);
            _recursoTarefaRepository.Setup(x => x.IncluirAsync(recursoTarefaMock));

            var pageModel = new IncluirModel(_recursoTarefaRepository.Object, _recursoProjetoRepository.Object, _tarefaRepository.Object);

            // Act
            var result = await pageModel.OnPostAsync(idTarefa);

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}
