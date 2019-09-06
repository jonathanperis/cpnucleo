using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.RecursoTarefa;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.RecursoTarefa
{
    public class RemoverTest
    {
        private readonly Mock<IRecursoTarefaRepository> _recursoTarefaRepository;
        private readonly Mock<ITarefaRepository> _tarefaRepository;

        public RemoverTest()
        {
            _recursoTarefaRepository = new Mock<IRecursoTarefaRepository>();
            _tarefaRepository = new Mock<ITarefaRepository>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idRecursoTarefa)
        {
            // Arrange
            var recursoTarefaMock = new RecursoTarefaModel { };

            _recursoTarefaRepository.Setup(x => x.ConsultarAsync(idRecursoTarefa)).ReturnsAsync(recursoTarefaMock);

            var pageModel = new RemoverModel(_recursoTarefaRepository.Object, _tarefaRepository.Object);

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
            var recursoTarefaMock = new RecursoTarefaModel { };

            _recursoTarefaRepository.Setup(x => x.RemoverAsync(recursoTarefaMock));

            var pageModel = new RemoverModel(_recursoTarefaRepository.Object, _tarefaRepository.Object);

            // Act
            var result = await pageModel.OnPostAsync(idTarefa);

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }
    }
}
