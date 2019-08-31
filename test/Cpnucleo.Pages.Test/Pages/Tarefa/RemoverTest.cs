using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Tarefa;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Tarefa
{
    public class RemoverTest
    {
        private readonly Mock<ITarefaRepository> _tarefaRepository;

        public RemoverTest() => _tarefaRepository = new Mock<ITarefaRepository>();

        [Theory]
        [InlineData(1)]
        public async Task Test_OnGetAsync(int idTarefa)
        {
            // Arrange
            var tarefaMock = new TarefaItem { };

            _tarefaRepository.Setup(x => x.ConsultarAsync(idTarefa)).ReturnsAsync(tarefaMock);

            var pageModel = new RemoverModel(_tarefaRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnGetAsync(idTarefa);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Theory]
        [InlineData(1)]
        public async Task Test_OnPostAsync(int idTarefa)
        {
            // Arrange
            var tarefaMock = new TarefaItem { IdTarefa = idTarefa };

            _tarefaRepository.Setup(x => x.RemoverAsync(tarefaMock));

            var pageModel = new RemoverModel(_tarefaRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }
    }
}
