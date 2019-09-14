using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Tarefa;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Tarefa
{
    public class ListarTest
    {
        private readonly Mock<ITarefaRepository> _tarefaRepository;

        public ListarTest() => _tarefaRepository = new Mock<ITarefaRepository>();

        [Fact]
        public void Test_OnGetAsync()
        {
            // Arrange
            var listaMock = new List<TarefaModel> { };

            _tarefaRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var pageModel = new ListarModel(_tarefaRepository.Object);

            var pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester

                // Assert
                .Action(x => x.OnGetAsync)
                .TestPage();
        }
    }
}
