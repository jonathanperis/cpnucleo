using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.RecursoProjeto;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.RecursoProjeto
{
    public class IncluirTest
    {
        private readonly Mock<IRecursoProjetoRepository> _recursoProjetoRepository;
        private readonly Mock<IRecursoRepository> _recursoRepository;
        private readonly Mock<IRepository<ProjetoModel>> _projetoRepository;

        public IncluirTest()
        {
            _recursoProjetoRepository = new Mock<IRecursoProjetoRepository>();
            _recursoRepository = new Mock<IRecursoRepository>();
            _projetoRepository = new Mock<IRepository<ProjetoModel>>();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnGetAsync(int idProjeto)
        {
            // Arrange
            var projetoMock = new ProjetoModel { };
            var listaMock = new List<RecursoModel> { };

            _projetoRepository.Setup(x => x.ConsultarAsync(idProjeto)).ReturnsAsync(projetoMock);
            _recursoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var pageModel = new IncluirModel(_recursoProjetoRepository.Object, _recursoRepository.Object, _projetoRepository.Object);

            var pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGetAsync(idProjeto))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnPostAsync(int idProjeto)
        {
            // Arrange
            var recursoProjetoMock = new RecursoProjetoModel { IdProjeto = idProjeto };
            var projetoMock = new ProjetoModel { };
            var listaMock = new List<RecursoModel> { };

            _recursoProjetoRepository.Setup(x => x.IncluirAsync(recursoProjetoMock));
            _projetoRepository.Setup(x => x.ConsultarAsync(idProjeto)).ReturnsAsync(projetoMock);
            _recursoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var pageModel = new IncluirModel(_recursoProjetoRepository.Object, _recursoRepository.Object, _projetoRepository.Object);

            var pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnPostAsync(idProjeto))

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => () => x.OnPostAsync(idProjeto))

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(recursoProjetoMock).ShouldReturn.NoErrors();
        }
    }
}
