using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.RecursoProjeto;
using Cpnucleo.Application.Interfaces;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System.Collections.Generic;
using Xunit;
using System;

namespace Cpnucleo.RazorPages.Test.Pages.RecursoProjeto
{
    public class IncluirTest
    {
        private readonly Mock<IRecursoProjetoAppService> _recursoProjetoAppService;
        private readonly Mock<IRecursoAppService> _recursoAppService;
        private readonly Mock<IAppService<ProjetoViewModel>> _projetoAppService;

        public IncluirTest()
        {
            _recursoProjetoAppService = new Mock<IRecursoProjetoAppService>();
            _recursoAppService = new Mock<IRecursoAppService>();
            _projetoAppService = new Mock<IAppService<ProjetoViewModel>>();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnGet(Guid idProjeto)
        {
            // Arrange
            var projetoMock = new ProjetoViewModel { };
            var listaMock = new List<RecursoViewModel> { };

            _projetoAppService.Setup(x => x.Consultar(idProjeto)).Returns(projetoMock);
            _recursoAppService.Setup(x => x.Listar()).Returns(listaMock);

            var pageModel = new IncluirModel(_recursoProjetoAppService.Object, _recursoAppService.Object, _projetoAppService.Object);

            var pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnGet(idProjeto))

                // Assert
                .TestPage();
        }

        [Theory]
        [InlineData(1)]
        public void Test_OnPost(Guid idProjeto)
        {
            // Arrange
            var recursoProjetoMock = new RecursoProjetoViewModel { IdProjeto = idProjeto };
            var projetoMock = new ProjetoViewModel { };
            var listaMock = new List<RecursoViewModel> { };

            _recursoProjetoAppService.Setup(x => x.Incluir(recursoProjetoMock));
            _projetoAppService.Setup(x => x.Consultar(idProjeto)).Returns(projetoMock);
            _recursoAppService.Setup(x => x.Listar()).Returns(listaMock);

            var pageModel = new IncluirModel(_recursoProjetoAppService.Object, _recursoAppService.Object, _projetoAppService.Object);

            var pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => () => x.OnPost(idProjeto))

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => () => x.OnPost(idProjeto))

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(recursoProjetoMock).ShouldReturn.NoErrors();
        }
    }
}
