using AutoMapper;
using Cpnucleo.Application.Handlers;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.CreateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.RemoveProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.UpdateProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.GetProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.ListProjeto;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
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

            await unitOfWork.SistemaRepository.AddAsync(new Sistema
            {
                Id = sistemaId,
                Nome = "Sistema de teste",
                Descricao = "Descrição do sistema de teste",
                DataInclusao = DateTime.Now,
                Ativo = true,
            });

            await unitOfWork.SaveChangesAsync();

            CreateProjetoCommand request = new()
            {
                Projeto = new ProjetoViewModel
                {
                    Id = Guid.NewGuid(),
                    Nome = "Projeto de teste",
                    IdSistema = sistemaId,
                }
            };

            // Act
            ProjetoHandler handler = new(unitOfWork, mapper);
            CreateProjetoResponse response = await handler.Handle(request, CancellationToken.None);

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

            await unitOfWork.SistemaRepository.AddAsync(new Sistema
            {
                Id = sistemaId,
                Nome = "Sistema de teste",
                Descricao = "Descrição do sistema de teste",
                DataInclusao = DateTime.Now,
                Ativo = true,
            });

            Guid projetoId = Guid.NewGuid();

            await unitOfWork.ProjetoRepository.AddAsync(new Projeto
            {
                Id = projetoId,
                Nome = "Projeto de teste",
                DataInclusao = DateTime.Now,
                Ativo = true,
                IdSistema = sistemaId
            });

            await unitOfWork.SaveChangesAsync();

            GetProjetoQuery request = new()
            {
                Id = projetoId
            };

            // Act
            ProjetoHandler handler = new(unitOfWork, mapper);
            GetProjetoResponse response = await handler.Handle(request, CancellationToken.None);

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

            await unitOfWork.SistemaRepository.AddAsync(new Sistema
            {
                Id = sistemaId,
                Nome = "Sistema de teste",
                Descricao = "Descrição do sistema de teste",
                DataInclusao = DateTime.Now,
                Ativo = true,
            });

            Guid projetoId = Guid.NewGuid();

            await unitOfWork.ProjetoRepository.AddAsync(new Projeto
            {
                Id = projetoId,
                Nome = "Projeto de teste 1",
                DataInclusao = DateTime.Now,
                Ativo = true,
                IdSistema = sistemaId
            });

            await unitOfWork.ProjetoRepository.AddAsync(new Projeto
            {
                Id = Guid.NewGuid(),
                Nome = "Projeto de teste 2",
                DataInclusao = DateTime.Now,
                Ativo = true,
                IdSistema = sistemaId
            });

            await unitOfWork.ProjetoRepository.AddAsync(new Projeto
            {
                Id = Guid.NewGuid(),
                Nome = "Projeto de teste 3",
                DataInclusao = DateTime.Now,
                Ativo = true,
                IdSistema = sistemaId
            });

            await unitOfWork.SaveChangesAsync();

            ListProjetoQuery request = new();

            // Act
            ProjetoHandler handler = new(unitOfWork, mapper);
            ListProjetoResponse response = await handler.Handle(request, CancellationToken.None);

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

            await unitOfWork.SistemaRepository.AddAsync(new Sistema
            {
                Id = sistemaId,
                Nome = "Sistema de teste",
                Descricao = "Descrição do sistema de teste",
                DataInclusao = DateTime.Now,
                Ativo = true,
            });

            Guid projetoId = Guid.NewGuid();

            Projeto projeto = new()
            {
                Id = projetoId,
                Nome = "Projeto de teste",
                DataInclusao = DateTime.Now,
                Ativo = true,
                IdSistema = sistemaId
            };

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
            RemoveProjetoResponse response = await handler.Handle(request, CancellationToken.None);
            GetProjetoResponse response2 = await handler.Handle(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.Success);
            Assert.True(response2.Projeto == null);
        }

        [Fact]
        public async Task UpdateProjetoCommand_Handle()
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

            Guid projetoId = Guid.NewGuid();
            DateTime dataInclusao = DateTime.Now;

            Projeto projeto = new()
            {
                Id = projetoId,
                Nome = "Projeto de teste",
                DataInclusao = dataInclusao,
                Ativo = true,
                IdSistema = sistemaId
            };

            await unitOfWork.ProjetoRepository.AddAsync(projeto);
            await unitOfWork.SaveChangesAsync();

            unitOfWork.ProjetoRepository.Detatch(projeto);

            UpdateProjetoCommand request = new()
            {
                Projeto = new ProjetoViewModel
                {
                    Id = projetoId,
                    Nome = "Projeto de teste - alterado",
                    DataInclusao = dataInclusao,
                    IdSistema = sistemaId,
                }
            };

            GetProjetoQuery request2 = new()
            {
                Id = projetoId
            };

            // Act
            ProjetoHandler handler = new(unitOfWork, mapper);
            UpdateProjetoResponse response = await handler.Handle(request, CancellationToken.None);
            GetProjetoResponse response2 = await handler.Handle(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.Success);
            Assert.True(response2.Projeto != null);
            Assert.True(response2.Projeto.Id == projetoId);
            Assert.True(response2.Projeto.DataInclusao.Ticks == dataInclusao.Ticks);
        }
    }
}
