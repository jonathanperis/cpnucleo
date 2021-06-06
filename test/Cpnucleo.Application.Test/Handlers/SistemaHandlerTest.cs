using AutoMapper;
using Cpnucleo.Application.Handlers;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.CreateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.RemoveSistema;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.UpdateSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.GetSistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.ListSistema;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Application.Test.Handlers
{
    public class SistemaHandlerTest
    {
        [Fact]
        public async Task CreateSistemaCommand_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            CreateSistemaCommand request = new()
            {
                Sistema = new SistemaViewModel
                {
                    Id = Guid.NewGuid(),
                    Nome = "Sistema de teste",
                    Descricao = "Descrição do sistema de teste",
                }
            };

            // Act
            SistemaHandler handler = new(unitOfWork, mapper);
            CreateSistemaResponse response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.Sistema != null);
            Assert.True(response.Sistema.Id != Guid.Empty);
            Assert.True(response.Sistema.DataInclusao.Ticks != 0);
        }

        [Fact]
        public async Task GetSistemaQuery_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid sistemaId = Guid.NewGuid();

            await unitOfWork.SistemaRepository.AddAsync(new Sistema
            {
                Id = sistemaId,
                Nome = "Sistema de teste",
                Descricao = "Descrição do sistema de teste",
                DataInclusao = DateTime.Now,
                Ativo = true,
            });

            await unitOfWork.SaveChangesAsync();

            GetSistemaQuery request = new()
            {
                Id = sistemaId
            };

            // Act
            SistemaHandler handler = new(unitOfWork, mapper);
            GetSistemaResponse response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.Sistema != null);
            Assert.True(response.Sistema.Id != Guid.Empty);
            Assert.True(response.Sistema.DataInclusao.Ticks != 0);
        }

        [Fact]
        public async Task ListSistemaQuery_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid sistemaId = Guid.NewGuid();

            await unitOfWork.SistemaRepository.AddAsync(new Sistema
            {
                Id = sistemaId,
                Nome = "Sistema de teste 1",
                Descricao = "Descrição do sistema de teste 1",
                DataInclusao = DateTime.Now,
                Ativo = true,
            });

            await unitOfWork.SistemaRepository.AddAsync(new Sistema
            {
                Id = Guid.NewGuid(),
                Nome = "Sistema de teste 2",
                Descricao = "Descrição do sistema de teste 2",
                DataInclusao = DateTime.Now,
                Ativo = true,
            });

            await unitOfWork.SistemaRepository.AddAsync(new Sistema
            {
                Id = Guid.NewGuid(),
                Nome = "Sistema de teste 3",
                Descricao = "Descrição do sistema de teste 3",
                DataInclusao = DateTime.Now,
                Ativo = true,
            });

            await unitOfWork.SaveChangesAsync();

            ListSistemaQuery request = new();

            // Act
            SistemaHandler handler = new(unitOfWork, mapper);
            ListSistemaResponse response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.Sistemas != null);
            Assert.True(response.Sistemas.Any());
            Assert.True(response.Sistemas.FirstOrDefault(x => x.Id == sistemaId) != null);
        }

        [Fact]
        public async Task RemoveSistemaCommand_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid sistemaId = Guid.NewGuid();

            await unitOfWork.SistemaRepository.AddAsync(new Sistema
            {
                Id = sistemaId,
                Nome = "Sistema de teste",
                Descricao = "Descrição do sistema de teste",
                DataInclusao = DateTime.Now,
                Ativo = true,
            });

            await unitOfWork.SaveChangesAsync();

            RemoveSistemaCommand request = new()
            {
                Id = sistemaId
            };

            GetSistemaQuery request2 = new()
            {
                Id = sistemaId
            };

            // Act
            SistemaHandler handler = new(unitOfWork, mapper);
            RemoveSistemaResponse response = await handler.Handle(request, CancellationToken.None);
            GetSistemaResponse response2 = await handler.Handle(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.Success);
            Assert.True(response2.Sistema == null);
        }

        [Fact]
        public async Task UpdateSistemaCommand_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid sistemaId = Guid.NewGuid();
            DateTime dataInclusao = DateTime.Now;

            await unitOfWork.SistemaRepository.AddAsync(new Sistema
            {
                Id = sistemaId,
                Nome = "Sistema de teste",
                Descricao = "Descrição do sistema de teste",
                DataInclusao = dataInclusao,
                Ativo = true,
            });

            await unitOfWork.SaveChangesAsync();

            UpdateSistemaCommand request = new()
            {
                Sistema = new SistemaViewModel 
                {
                    Id = sistemaId,
                    Nome = "Sistema de teste - alterado",
                    Descricao = "Descrição do sistema de teste - alterado",
                    DataInclusao = dataInclusao
                }
            };

            GetSistemaQuery request2 = new()
            {
                Id = sistemaId
            };

            // Act
            SistemaHandler handler = new(unitOfWork, mapper);
            UpdateSistemaResponse response = await handler.Handle(request, CancellationToken.None);
            GetSistemaResponse response2 = await handler.Handle(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Sistema != null);
            Assert.True(response2.Sistema.Id == sistemaId);
            Assert.True(response2.Sistema.DataInclusao.Ticks == dataInclusao.Ticks);
        }
    }
}
