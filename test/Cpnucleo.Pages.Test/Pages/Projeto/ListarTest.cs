using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Projeto;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Projeto
{
    public class ListarTest
    {
        private readonly Mock<IRepository<ProjetoModel>> _projetoRepository;

        public ListarTest() => _projetoRepository = new Mock<IRepository<ProjetoModel>>();

        [Fact]
        public void Test_OnGetAsync()
        {
            // Arrange
            var listaMock = new List<ProjetoModel> { };

            _projetoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var pageModel = new ListarModel(_projetoRepository.Object);

            var pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester

                // Assert
                .Action(x => x.OnGetAsync)
                .TestPage();
        }
    }
}
