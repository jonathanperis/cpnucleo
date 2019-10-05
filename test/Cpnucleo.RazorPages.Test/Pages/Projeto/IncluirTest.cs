using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.RazorPages.Pages.Projeto;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System;
using System.Collections.Generic;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Projeto
{
    public class IncluirTest
    {
        private readonly Mock<ICrudAppService<ProjetoViewModel>> _projetoAppService;
        private readonly Mock<ICrudAppService<SistemaViewModel>> _sistemaAppService;

        public IncluirTest()
        {
            _projetoAppService = new Mock<ICrudAppService<ProjetoViewModel>>();
            _sistemaAppService = new Mock<ICrudAppService<SistemaViewModel>>();
        }

        [Theory]
        [InlineData("Projeto de Teste")]
        public void Test_OnPost(string nome)
        {
            // Arrange
            Guid idSistema = Guid.NewGuid();

            ProjetoViewModel projetoMock = new ProjetoViewModel { Nome = nome, IdSistema = idSistema };
            List<SistemaViewModel> listaMock = new List<SistemaViewModel> { };

            _projetoAppService.Setup(x => x.Incluir(projetoMock));
            _sistemaAppService.Setup(x => x.Listar()).Returns(listaMock);

            IncluirModel pageModel = new IncluirModel(_projetoAppService.Object, _sistemaAppService.Object);
            PageModelTester<IncluirModel> pageTester = new PageModelTester<IncluirModel>(pageModel);

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .WhenModelStateIsValidEquals(false)
                .TestPage();

            // Act
            pageTester
                .Action(x => x.OnPost)

                // Assert
                .WhenModelStateIsValidEquals(true)
                .TestRedirectToPage("Listar");

            // Assert
            Validation.For(projetoMock).ShouldReturn.NoErrors();
        }
    }
}
