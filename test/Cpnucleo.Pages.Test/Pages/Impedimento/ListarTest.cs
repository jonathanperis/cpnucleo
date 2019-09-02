using Cpnucleo.Pages.Models;
using Cpnucleo.Pages.Pages.Impedimento;
using Cpnucleo.Pages.Repository;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.Pages.Test.Pages.Impedimento
{
    public class ListarTest
    {
        private readonly Mock<IRepository<ImpedimentoItem>> _impedimentoRepository;

        public ListarTest() => _impedimentoRepository = new Mock<IRepository<ImpedimentoItem>>();

        [Fact]
        public void Test_OnGetAsync()
        {
            // Arrange
            var listaMock = new List<ImpedimentoItem> { };

            _impedimentoRepository.Setup(x => x.ListarAsync()).ReturnsAsync(listaMock);

            var pageModel = new ListarModel(_impedimentoRepository.Object);

            var pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester

                // Assert
                .Action(x => x.OnGetAsync)
                .TestPage();
        }
    }
}
