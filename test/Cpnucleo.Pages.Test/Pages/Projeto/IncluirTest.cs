using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Projeto;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Projeto
{
    public class IncluirTest
    {
        private readonly Mock<IRepository<ProjetoModel>> _projetoRepository;
        private readonly Mock<IRepository<SistemaModel>> _sistemaRepository;

        public IncluirTest()
        {
            _projetoRepository = new Mock<IRepository<ProjetoModel>>();
            _sistemaRepository = new Mock<IRepository<SistemaModel>>();
        }

        [Theory]
        [InlineData("Projeto de Teste", 1)]
        public void Test_OnPostAsync(string nome, int idSistema)
        {
            // Arrange
            var projetoMock = new ProjetoModel { Nome = nome, IdSistema = idSistema };
            var listaMock = new List<SistemaModel> { };

            _projetoRepository.Setup(x => x.IncluirAsync(projetoMock));
            _sistemaRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var pageModel = new IncluirModel(_projetoRepository.Object, _sistemaRepository.Object);

            var pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPostAsync)

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => x.OnPostAsync)

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(projetoMock).ShouldReturn.NoErrors();
        }
    }
}
