using AutoMapper;
using Cpnucleo.Application.Handlers;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.CreateTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.RemoveTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.UpdateTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.GetTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa.ListTipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Application.Test.Handlers
{
    public class TipoTarefaHandlerTest
    {
        [Fact]
        public async Task CreateTipoTarefaCommand_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            CreateTipoTarefaCommand request = new()
            {
                TipoTarefa = new TipoTarefaViewModel
                {
                    Id = Guid.NewGuid(),
                    Nome = "TipoTarefa de teste",
                    Element = "success-element",
                    Image = "success.png"
                }
            };

            // Act
            TipoTarefaHandler handler = new(unitOfWork, mapper);
            CreateTipoTarefaResponse response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.TipoTarefa != null);
            Assert.True(response.TipoTarefa.Id != Guid.Empty);
            Assert.True(response.TipoTarefa.DataInclusao.Ticks != 0);
        }

        [Fact]
        public async Task GetTipoTarefaQuery_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid tipoTarefaId = Guid.NewGuid();

            await unitOfWork.TipoTarefaRepository.AddAsync(new TipoTarefa
            {
                Id = tipoTarefaId,
                Nome = "TipoTarefa de teste",
                Element = "success-element",
                Image = "success.png",
                DataInclusao = DateTime.Now,
                Ativo = true,
            });

            await unitOfWork.SaveChangesAsync();

            GetTipoTarefaQuery request = new()
            {
                Id = tipoTarefaId
            };

            // Act
            TipoTarefaHandler handler = new(unitOfWork, mapper);
            GetTipoTarefaResponse response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.TipoTarefa != null);
            Assert.True(response.TipoTarefa.Id != Guid.Empty);
            Assert.True(response.TipoTarefa.DataInclusao.Ticks != 0);
        }

        [Fact]
        public async Task ListTipoTarefaQuery_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid tipoTarefaId = Guid.NewGuid();

            await unitOfWork.TipoTarefaRepository.AddAsync(new TipoTarefa
            {
                Id = tipoTarefaId,
                Nome = "TipoTarefa de teste 1",
                Element = "success-element",
                Image = "success.png",
                DataInclusao = DateTime.Now,
                Ativo = true,
            });

            await unitOfWork.TipoTarefaRepository.AddAsync(new TipoTarefa
            {
                Id = Guid.NewGuid(),
                Nome = "TipoTarefa de teste 2",
                Element = "success-element",
                Image = "success.png",
                DataInclusao = DateTime.Now,
                Ativo = true,
            });

            await unitOfWork.TipoTarefaRepository.AddAsync(new TipoTarefa
            {
                Id = Guid.NewGuid(),
                Nome = "TipoTarefa de teste 3",
                Element = "success-element",
                Image = "success.png",
                DataInclusao = DateTime.Now,
                Ativo = true,
            });

            await unitOfWork.SaveChangesAsync();

            ListTipoTarefaQuery request = new();

            // Act
            TipoTarefaHandler handler = new(unitOfWork, mapper);
            ListTipoTarefaResponse response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.TipoTarefas != null);
            Assert.True(response.TipoTarefas.Any());
            Assert.True(response.TipoTarefas.FirstOrDefault(x => x.Id == tipoTarefaId) != null);
        }

        [Fact]
        public async Task RemoveTipoTarefaCommand_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid tipoTarefaId = Guid.NewGuid();

            TipoTarefa tipoTarefa = new()
            {
                Id = tipoTarefaId,
                Nome = "TipoTarefa de teste",
                Element = "success-element",
                Image = "success.png",
                DataInclusao = DateTime.Now,
                Ativo = true,
            };

            await unitOfWork.TipoTarefaRepository.AddAsync(tipoTarefa);
            await unitOfWork.SaveChangesAsync();

            unitOfWork.TipoTarefaRepository.Detatch(tipoTarefa);

            RemoveTipoTarefaCommand request = new()
            {
                Id = tipoTarefaId
            };

            GetTipoTarefaQuery request2 = new()
            {
                Id = tipoTarefaId
            };

            // Act
            TipoTarefaHandler handler = new(unitOfWork, mapper);
            RemoveTipoTarefaResponse response = await handler.Handle(request, CancellationToken.None);
            GetTipoTarefaResponse response2 = await handler.Handle(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.NotFound);
            Assert.True(response2.TipoTarefa == null);
        }

        [Fact]
        public async Task UpdateTipoTarefaCommand_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid tipoTarefaId = Guid.NewGuid();
            DateTime dataInclusao = DateTime.Now;

            TipoTarefa tipoTarefa = new()
            {
                Id = tipoTarefaId,
                Nome = "TipoTarefa de teste",
                Element = "success-element",
                Image = "success.png",
                DataInclusao = dataInclusao,
                Ativo = true,
            };

            await unitOfWork.TipoTarefaRepository.AddAsync(tipoTarefa);
            await unitOfWork.SaveChangesAsync();

            unitOfWork.TipoTarefaRepository.Detatch(tipoTarefa);

            UpdateTipoTarefaCommand request = new()
            {
                TipoTarefa = new TipoTarefaViewModel
                {
                    Id = tipoTarefaId,
                    Nome = "TipoTarefa de teste - alterado",

                    DataInclusao = dataInclusao
                }
            };

            GetTipoTarefaQuery request2 = new()
            {
                Id = tipoTarefaId
            };

            // Act
            TipoTarefaHandler handler = new(unitOfWork, mapper);
            UpdateTipoTarefaResponse response = await handler.Handle(request, CancellationToken.None);
            GetTipoTarefaResponse response2 = await handler.Handle(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.Success);
            Assert.True(response2.TipoTarefa != null);
            Assert.True(response2.TipoTarefa.Id == tipoTarefaId);
            Assert.True(response2.TipoTarefa.DataInclusao.Ticks == dataInclusao.Ticks);
        }
    }
}
