using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.RecursoTarefa;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
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
        public void Test_OnGetAsync(int idRecursoTarefa)
        {
            // Arrange
            var recursoTarefaMock = new RecursoTarefaModel { };

            _recursoTarefaRepository.Setup(x => x.ConsultarAsync(idRecursoTarefa)).ReturnsAsync(recursoTarefaMock);

            var pageModel = new RemoverModel(_recursoTarefaRepository.Object, _tarefaRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGetAsync(idRecursoTarefa))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnPostAsync(int idTarefa)
        {
            // Arrange
            var recursoTarefaMock = new RecursoTarefaModel { };

            _recursoTarefaRepository.Setup(x => x.RemoverAsync(recursoTarefaMock));

            var pageModel = new RemoverModel(_recursoTarefaRepository.Object, _tarefaRepository.Object);

            var pageTester = new PageModelTester<RemoverModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnPostAsync(idTarefa))

                // Assert
                .TestRedirectToPage("Listar");
        }
    }
}
