using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Sistema;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Sistema
{
    public class ListarTest
    {
        private readonly Mock<IRepository<SistemaModel>> _sistemaRepository;

        public ListarTest() => _sistemaRepository = new Mock<IRepository<SistemaModel>>();

        [Fact]
        public void Test_OnGetAsync()
        {
            // Arrange
            var listaMock = new List<SistemaModel> { };

            _sistemaRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var pageModel = new ListarModel(_sistemaRepository.Object);

            var pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester

                // Assert
                .Action(x => x.OnGetAsync)
                .TestPage();
        }
    }
}
