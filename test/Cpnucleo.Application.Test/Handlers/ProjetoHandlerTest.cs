using AutoMapper;
using Cpnucleo.Application.Handlers;
using Cpnucleo.Application.Test.Helpers;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.CreateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.RemoveProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.UpdateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.GetProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.ListProjeto;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Application.Test.Handlers
{
    public class ProjetoHandlerTest
    {
        [Fact]
        public async Task CreateProjetoCommand_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid sistemaId = Guid.NewGuid();

            await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));
            await unitOfWork.SaveChangesAsync();

            CreateProjetoCommand request = new()
            {
                Projeto = MockViewModelHelper.GetNewProjeto(sistemaId)
            };

            // Act
            ProjetoHandler handler = new(unitOfWork, mapper);
            CreateProjetoResponse response = await handler.InvokeAsync(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.Projeto != null);
            Assert.True(response.Projeto.Id != Guid.Empty);
            Assert.True(response.Projeto.DataInclusao.Ticks != 0);
        }

        [Fact]
        public async Task GetProjetoQuery_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid sistemaId = Guid.NewGuid();

            await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

            Guid projetoId = Guid.NewGuid();

            await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));
            await unitOfWork.SaveChangesAsync();

            GetProjetoQuery request = new()
            {
                Id = projetoId
            };

            // Act
            ProjetoHandler handler = new(unitOfWork, mapper);
            GetProjetoResponse response = await handler.InvokeAsync(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.Projeto != null);
            Assert.True(response.Projeto.Id != Guid.Empty);
            Assert.True(response.Projeto.DataInclusao.Ticks != 0);
        }

        [Fact]
        public async Task ListProjetoQuery_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid sistemaId = Guid.NewGuid();

            await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

            Guid projetoId = Guid.NewGuid();

            await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));
            await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId));
            await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId));

            await unitOfWork.SaveChangesAsync();

            ListProjetoQuery request = new();

            // Act
            ProjetoHandler handler = new(unitOfWork, mapper);
            ListProjetoResponse response = await handler.InvokeAsync(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.Projetos != null);
            Assert.True(response.Projetos.Any());
            Assert.True(response.Projetos.FirstOrDefault(x => x.Id == projetoId) != null);
        }

        [Fact]
        public async Task RemoveProjetoCommand_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid sistemaId = Guid.NewGuid();

            await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

            Guid projetoId = Guid.NewGuid();

            Projeto projeto = MockEntityHelper.GetNewProjeto(sistemaId, projetoId);

            await unitOfWork.ProjetoRepository.AddAsync(projeto);
            await unitOfWork.SaveChangesAsync();

            unitOfWork.ProjetoRepository.Detatch(projeto);

            RemoveProjetoCommand request = new()
            {
                Id = projetoId
            };

            GetProjetoQuery request2 = new()
            {
                Id = projetoId
            };

            // Act
            ProjetoHandler handler = new(unitOfWork, mapper);
            RemoveProjetoResponse response = await handler.InvokeAsync(request, CancellationToken.None);
            GetProjetoResponse response2 = await handler.InvokeAsync(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.NotFound);
            Assert.True(response2.Projeto == null);
        }

        [Fact]
        public async Task UpdateProjetoCommand_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid sistemaId = Guid.NewGuid();

            await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

            Guid projetoId = Guid.NewGuid();
            DateTime dataInclusao = DateTime.Now;

            Projeto projeto = MockEntityHelper.GetNewProjeto(sistemaId, projetoId);

            await unitOfWork.ProjetoRepository.AddAsync(projeto);
            await unitOfWork.SaveChangesAsync();

            unitOfWork.ProjetoRepository.Detatch(projeto);

            UpdateProjetoCommand request = new()
            {
                Projeto = MockViewModelHelper.GetNewProjeto(sistemaId, projetoId, dataInclusao)
            };

            GetProjetoQuery request2 = new()
            {
                Id = projetoId
            };

            // Act
            ProjetoHandler handler = new(unitOfWork, mapper);
            UpdateProjetoResponse response = await handler.InvokeAsync(request, CancellationToken.None);
            GetProjetoResponse response2 = await handler.InvokeAsync(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.Success);
            Assert.True(response2.Projeto != null);
            Assert.True(response2.Projeto.Id == projetoId);
            Assert.True(response2.Projeto.DataInclusao.Ticks == dataInclusao.Ticks);
        }
    }
}
