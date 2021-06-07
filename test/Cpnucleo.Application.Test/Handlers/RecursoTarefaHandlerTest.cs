using AutoMapper;
using Cpnucleo.Application.Handlers;
using Cpnucleo.Application.Test.Helpers;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.CreateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.RemoveRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.UpdateRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetRecursoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.ListRecursoTarefa;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Application.Test.Handlers
{
    public class RecursoTarefaHandlerTest
    {
        [Fact]
        public async Task CreateRecursoTarefaCommand_Handle()
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

            Guid tipoTarefaId = Guid.NewGuid();
            await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa(tipoTarefaId));

            Guid tarefaId = Guid.NewGuid();
            await unitOfWork.TarefaRepository.AddAsync(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId));

            await unitOfWork.SaveChangesAsync();

            CreateRecursoTarefaCommand request = new()
            {
                RecursoTarefa = MockViewModelHelper.GetNewRecursoTarefa(tarefaId, recursoId)
            };

            // Act
            RecursoTarefaHandler handler = new(unitOfWork, mapper);
            CreateRecursoTarefaResponse response = await handler.InvokeAsync(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.RecursoTarefa != null);
            Assert.True(response.RecursoTarefa.Id != Guid.Empty);
            Assert.True(response.RecursoTarefa.DataInclusao.Ticks != 0);
        }

        [Fact]
        public async Task GetRecursoTarefaQuery_Handle()
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

            Guid tipoTarefaId = Guid.NewGuid();
            await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa(tipoTarefaId));

            Guid tarefaId = Guid.NewGuid();
            await unitOfWork.TarefaRepository.AddAsync(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId));

            Guid recursoTarefaId = Guid.NewGuid();
            await unitOfWork.RecursoTarefaRepository.AddAsync(MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId, recursoTarefaId));

            await unitOfWork.SaveChangesAsync();

            GetRecursoTarefaQuery request = new()
            {
                Id = recursoTarefaId
            };

            // Act
            RecursoTarefaHandler handler = new(unitOfWork, mapper);
            GetRecursoTarefaResponse response = await handler.InvokeAsync(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.RecursoTarefa != null);
            Assert.True(response.RecursoTarefa.Id != Guid.Empty);
            Assert.True(response.RecursoTarefa.DataInclusao.Ticks != 0);
        }

        [Fact]
        public async Task ListRecursoTarefaQuery_Handle()
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

            Guid tipoTarefaId = Guid.NewGuid();
            await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa(tipoTarefaId));

            Guid tarefaId = Guid.NewGuid();
            await unitOfWork.TarefaRepository.AddAsync(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId));

            Guid recursoTarefaId = Guid.NewGuid();
            await unitOfWork.RecursoTarefaRepository.AddAsync(MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId, recursoTarefaId));
            await unitOfWork.RecursoTarefaRepository.AddAsync(MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId));
            await unitOfWork.RecursoTarefaRepository.AddAsync(MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId));

            await unitOfWork.SaveChangesAsync();

            ListRecursoTarefaQuery request = new();

            // Act
            RecursoTarefaHandler handler = new(unitOfWork, mapper);
            ListRecursoTarefaResponse response = await handler.InvokeAsync(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.RecursoTarefas != null);
            Assert.True(response.RecursoTarefas.Any());
            Assert.True(response.RecursoTarefas.FirstOrDefault(x => x.Id == recursoTarefaId) != null);
        }

        [Fact]
        public async Task RemoveRecursoTarefaCommand_Handle()
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

            Guid tipoTarefaId = Guid.NewGuid();
            await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa(tipoTarefaId));

            Guid tarefaId = Guid.NewGuid();
            await unitOfWork.TarefaRepository.AddAsync(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId));

            Guid recursoTarefaId = Guid.NewGuid();
            RecursoTarefa recursoTarefa = MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId, recursoTarefaId);

            await unitOfWork.RecursoTarefaRepository.AddAsync(recursoTarefa);
            await unitOfWork.SaveChangesAsync();

            unitOfWork.RecursoTarefaRepository.Detatch(recursoTarefa);

            RemoveRecursoTarefaCommand request = new()
            {
                Id = recursoTarefaId
            };

            GetRecursoTarefaQuery request2 = new()
            {
                Id = recursoTarefaId
            };

            // Act
            RecursoTarefaHandler handler = new(unitOfWork, mapper);
            RemoveRecursoTarefaResponse response = await handler.InvokeAsync(request, CancellationToken.None);
            GetRecursoTarefaResponse response2 = await handler.InvokeAsync(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.NotFound);
            Assert.True(response2.RecursoTarefa == null);
        }

        [Fact]
        public async Task UpdateRecursoTarefaCommand_Handle()
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

            Guid tipoTarefaId = Guid.NewGuid();
            await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa(tipoTarefaId));

            Guid tarefaId = Guid.NewGuid();
            await unitOfWork.TarefaRepository.AddAsync(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId));

            Guid recursoTarefaId = Guid.NewGuid();
            DateTime dataInclusao = DateTime.Now;
            RecursoTarefa recursoTarefa = MockEntityHelper.GetNewRecursoTarefa(tarefaId, recursoId, recursoTarefaId);

            await unitOfWork.RecursoTarefaRepository.AddAsync(recursoTarefa);
            await unitOfWork.SaveChangesAsync();

            unitOfWork.RecursoTarefaRepository.Detatch(recursoTarefa);

            UpdateRecursoTarefaCommand request = new()
            {
                RecursoTarefa = MockViewModelHelper.GetNewRecursoTarefa(tarefaId, recursoId, recursoTarefaId, dataInclusao)
            };

            GetRecursoTarefaQuery request2 = new()
            {
                Id = recursoTarefaId
            };

            // Act
            RecursoTarefaHandler handler = new(unitOfWork, mapper);
            UpdateRecursoTarefaResponse response = await handler.InvokeAsync(request, CancellationToken.None);
            GetRecursoTarefaResponse response2 = await handler.InvokeAsync(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.Success);
            Assert.True(response2.RecursoTarefa != null);
            Assert.True(response2.RecursoTarefa.Id == recursoTarefaId);
            Assert.True(response2.RecursoTarefa.DataInclusao.Ticks == dataInclusao.Ticks);
        }
    }
}
