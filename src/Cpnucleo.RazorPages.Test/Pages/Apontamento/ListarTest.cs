using Cpnucleo.Application.Interfaces;
using Cpnucleo.Infra.CrossCutting.Identity.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.RazorPages.Pages.Apontamento;
using Moq;
using SparkyTestHelpers.AspNetMvc.Core;
using SparkyTestHelpers.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace Cpnucleo.RazorPages.Test.Pages.Apontamento
{
    public class ListarTest
    {
        private readonly Mock<IClaimsManager> _claimsManager;
        private readonly Mock<IApontamentoAppService> _apontamentoAppService;
        private readonly Mock<IRecursoTarefaAppService> _recursoTarefaAppService;

        public ListarTest()
        {
            _claimsManager = new Mock<IClaimsManager>();
            _apontamentoAppService = new Mock<IApontamentoAppService>();
            _recursoTarefaAppService = new Mock<IRecursoTarefaAppService>();
        }

        [Fact]
        public void Test_OnGet()
        {
            // Arrange
            Guid idRecurso = Guid.NewGuid();

            string retorno = "e91df56a-09b1-4f14-abf4-5b098f4e404b";
            List<ApontamentoViewModel> listaMock = new List<ApontamentoViewModel> { };
            List<RecursoTarefaViewModel> listaRecursoTarefaMock = new List<RecursoTarefaViewModel> { };

            ListarModel pageModel = new ListarModel(_claimsManager.Object, _apontamentoAppService.Object, _recursoTarefaAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _claimsManager.Setup(x => x.ReadClaimsPrincipal(pageModel.HttpContext.User, ClaimTypes.PrimarySid)).Returns(retorno);
            _apontamentoAppService.Setup(x => x.ListarPorRecurso(idRecurso)).Returns(listaMock);
            _recursoTarefaAppService.Setup(x => x.ListarPorRecurso(idRecurso)).Returns(listaRecursoTarefaMock);
            
            PageModelTester<ListarModel> pageTester = new PageModelTester<ListarModel>(pageModel);

            // Act
            pageTester

                // Assert
                .Action(x => x.OnGet)
                .TestPage();
        }

        [Theory]
        [InlineData("Descrição de Teste", 8, 55)]
        public void Test_OnPost(string descricao, int qtdHoras, int percentualConcluido)
        {
            // Arrange
            Guid idRecurso = Guid.NewGuid();
            Guid idTarefa = Guid.NewGuid();

            DateTime dataApontamento = DateTime.Now;

            ApontamentoViewModel apontamentoMock = new ApontamentoViewModel { Descricao = descricao, DataApontamento = dataApontamento, QtdHoras = qtdHoras, PercentualConcluido = percentualConcluido, IdTarefa = idTarefa };

            string retorno = "e91df56a-09b1-4f14-abf4-5b098f4e404b";
            List<ApontamentoViewModel> listaMock = new List<ApontamentoViewModel> { };
            List<RecursoTarefaViewModel> listaRecursoTarefaMock = new List<RecursoTarefaViewModel> { };

            ListarModel pageModel = new ListarModel(_claimsManager.Object, _apontamentoAppService.Object, _recursoTarefaAppService.Object)
            {
                PageContext = PageContextManager.CreatePageContext()
            };

            _claimsManager.Setup(x => x.ReadClaimsPrincipal(pageModel.HttpContext.User, ClaimTypes.PrimarySid)).Returns(retorno);
            _apontamentoAppService.Setup(x => x.ListarPorRecurso(idRecurso)).Returns(listaMock);
            _recursoTarefaAppService.Setup(x => x.ListarPorRecurso(idRecurso)).Returns(listaRecursoTarefaMock);

            PageModelTester<ListarModel> pageTester = new PageModelTester<ListarModel>(pageModel);

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
            Validation.For(apontamentoMock).ShouldReturn.NoErrors();
        }
    }
}
