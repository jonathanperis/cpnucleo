using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Tarefa;
using Cpnucleo.Pages.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Tarefa
{
    public class ListarTest
    {
        private readonly Mock<ITarefaRepository> _tarefaRepository;

        public ListarTest() => _tarefaRepository = new Mock<ITarefaRepository>();

        [Fact]
        public async Task Test_OnGetAsync()
        {
            // Arrange
            var listaMock = new List<TarefaItem> { };

            _tarefaRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var pageModel = new ListarModel(_tarefaRepository.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            // Act
            var result = await pageModel.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }
    }
}
