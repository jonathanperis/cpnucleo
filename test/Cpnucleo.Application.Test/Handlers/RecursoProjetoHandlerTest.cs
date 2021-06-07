using AutoMapper;
using Cpnucleo.Application.Handlers;
using Cpnucleo.Application.Test.Helpers;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.CreateRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.RemoveRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.UpdateRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.ListRecursoProjeto;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Application.Test.Handlers
{
    public class RecursoProjetoHandlerTest
    {
        [Fact]
        public async Task CreateRecursoProjetoCommand_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid sistemaId = Guid.NewGuid();
            await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

            Guid projetoId = Guid.NewGuid();
            await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));

            Guid workflowId = Guid.NewGuid();
            await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));

            Guid recursoId = Guid.NewGuid();
            await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));

            await unitOfWork.SaveChangesAsync();

            CreateRecursoProjetoCommand request = new()
            {
                RecursoProjeto = MockViewModelHelper.GetNewRecursoProjeto(projetoId, recursoId)
            };

            // Act
            RecursoProjetoHandler handler = new(unitOfWork, mapper);
            CreateRecursoProjetoResponse response = await handler.InvokeAsync(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.RecursoProjeto != null);
            Assert.True(response.RecursoProjeto.Id != Guid.Empty);
            Assert.True(response.RecursoProjeto.DataInclusao.Ticks != 0);
        }

        [Fact]
        public async Task GetRecursoProjetoQuery_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid sistemaId = Guid.NewGuid();
            await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

            Guid projetoId = Guid.NewGuid();
            await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));

            Guid workflowId = Guid.NewGuid();
            await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));

            Guid recursoId = Guid.NewGuid();
            await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));

            Guid recursoProjetoId = Guid.NewGuid();
            await unitOfWork.RecursoProjetoRepository.AddAsync(MockEntityHelper.GetNewRecursoProjeto(projetoId, recursoId, recursoProjetoId));

            await unitOfWork.SaveChangesAsync();

            GetRecursoProjetoQuery request = new()
            {
                Id = recursoProjetoId
            };

            // Act
            RecursoProjetoHandler handler = new(unitOfWork, mapper);
            GetRecursoProjetoResponse response = await handler.InvokeAsync(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.RecursoProjeto != null);
            Assert.True(response.RecursoProjeto.Id != Guid.Empty);
            Assert.True(response.RecursoProjeto.DataInclusao.Ticks != 0);
        }

        [Fact]
        public async Task ListRecursoProjetoQuery_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid sistemaId = Guid.NewGuid();
            await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

            Guid projetoId = Guid.NewGuid();
            await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));

            Guid workflowId = Guid.NewGuid();
            await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));

            Guid recursoId = Guid.NewGuid();
            await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));

            Guid recursoProjetoId = Guid.NewGuid();
            await unitOfWork.RecursoProjetoRepository.AddAsync(MockEntityHelper.GetNewRecursoProjeto(projetoId, recursoId, recursoProjetoId));
            await unitOfWork.RecursoProjetoRepository.AddAsync(MockEntityHelper.GetNewRecursoProjeto(projetoId, recursoId));
            await unitOfWork.RecursoProjetoRepository.AddAsync(MockEntityHelper.GetNewRecursoProjeto(projetoId, recursoId));

            await unitOfWork.SaveChangesAsync();

            ListRecursoProjetoQuery request = new();

            // Act
            RecursoProjetoHandler handler = new(unitOfWork, mapper);
            ListRecursoProjetoResponse response = await handler.InvokeAsync(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.RecursoProjetos != null);
            Assert.True(response.RecursoProjetos.Any());
            Assert.True(response.RecursoProjetos.FirstOrDefault(x => x.Id == recursoProjetoId) != null);
        }

        [Fact]
        public async Task RemoveRecursoProjetoCommand_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid sistemaId = Guid.NewGuid();
            await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

            Guid projetoId = Guid.NewGuid();
            await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));

            Guid workflowId = Guid.NewGuid();
            await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));

            Guid recursoId = Guid.NewGuid();
            await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));

            Guid recursoProjetoId = Guid.NewGuid();
            RecursoProjeto recursoProjeto = MockEntityHelper.GetNewRecursoProjeto(projetoId, recursoId, recursoProjetoId);

            await unitOfWork.RecursoProjetoRepository.AddAsync(recursoProjeto);
            await unitOfWork.SaveChangesAsync();

            unitOfWork.RecursoProjetoRepository.Detatch(recursoProjeto);

            RemoveRecursoProjetoCommand request = new()
            {
                Id = recursoProjetoId
            };

            GetRecursoProjetoQuery request2 = new()
            {
                Id = recursoProjetoId
            };

            // Act
            RecursoProjetoHandler handler = new(unitOfWork, mapper);
            RemoveRecursoProjetoResponse response = await handler.InvokeAsync(request, CancellationToken.None);
            GetRecursoProjetoResponse response2 = await handler.InvokeAsync(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.NotFound);
            Assert.True(response2.RecursoProjeto == null);
        }

        [Fact]
        public async Task UpdateRecursoProjetoCommand_Handle()
        {
            // Arrange
            IUnitOfWork unitOfWork = DbContextHelper.GetContext();
            IMapper mapper = AutoMapperHelper.GetMappings();

            Guid sistemaId = Guid.NewGuid();
            await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

            Guid projetoId = Guid.NewGuid();
            await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));

            Guid workflowId = Guid.NewGuid();
            await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));

            Guid recursoId = Guid.NewGuid();
            await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));

            Guid recursoProjetoId = Guid.NewGuid();
            DateTime dataInclusao = DateTime.Now;
            RecursoProjeto recursoProjeto = MockEntityHelper.GetNewRecursoProjeto(projetoId, recursoId, recursoProjetoId);

            await unitOfWork.RecursoProjetoRepository.AddAsync(recursoProjeto);
            await unitOfWork.SaveChangesAsync();

            unitOfWork.RecursoProjetoRepository.Detatch(recursoProjeto);

            UpdateRecursoProjetoCommand request = new()
            {
                RecursoProjeto = MockViewModelHelper.GetNewRecursoProjeto(projetoId, recursoId, recursoProjetoId, dataInclusao)
            };

            GetRecursoProjetoQuery request2 = new()
            {
                Id = recursoProjetoId
            };

            // Act
            RecursoProjetoHandler handler = new(unitOfWork, mapper);
            UpdateRecursoProjetoResponse response = await handler.InvokeAsync(request, CancellationToken.None);
            GetRecursoProjetoResponse response2 = await handler.InvokeAsync(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.Success);
            Assert.True(response2.RecursoProjeto != null);
            Assert.True(response2.RecursoProjeto.Id == recursoProjetoId);
            Assert.True(response2.RecursoProjeto.DataInclusao.Ticks == dataInclusao.Ticks);
        }
    }
}
