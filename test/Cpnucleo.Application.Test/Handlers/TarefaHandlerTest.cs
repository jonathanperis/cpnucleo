using AutoMapper;
using Cpnucleo.Application.Handlers;
using Cpnucleo.Application.Test.Helpers;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.CreateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.RemoveTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.UpdateTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.ListTarefa;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Application.Test.Handlers
{
    public class TarefaHandlerTest
    {
        [Fact]
        public async Task CreateTarefaCommand_Handle()
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

            await unitOfWork.SaveChangesAsync();

            CreateTarefaCommand request = new()
            {
                Tarefa = MockViewModelHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId)
            };

            // Act
            TarefaHandler handler = new(unitOfWork, mapper);
            CreateTarefaResponse response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.Tarefa != null);
            Assert.True(response.Tarefa.Id != Guid.Empty);
            Assert.True(response.Tarefa.DataInclusao.Ticks != 0);
        }

        [Fact]
        public async Task GetTarefaQuery_Handle()
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

            GetTarefaQuery request = new()
            {
                Id = tarefaId
            };

            // Act
            TarefaHandler handler = new(unitOfWork, mapper);
            GetTarefaResponse response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.Tarefa != null);
            Assert.True(response.Tarefa.Id != Guid.Empty);
            Assert.True(response.Tarefa.DataInclusao.Ticks != 0);
        }

        [Fact]
        public async Task ListTarefaQuery_Handle()
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
            await unitOfWork.TarefaRepository.AddAsync(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId));
            await unitOfWork.TarefaRepository.AddAsync(MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId));

            await unitOfWork.SaveChangesAsync();

            ListTarefaQuery request = new();

            // Act
            TarefaHandler handler = new(unitOfWork, mapper);
            ListTarefaResponse response = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.Tarefas != null);
            Assert.True(response.Tarefas.Any());
            Assert.True(response.Tarefas.FirstOrDefault(x => x.Id == tarefaId) != null);
        }

        [Fact]
        public async Task RemoveTarefaCommand_Handle()
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
            Tarefa tarefa = MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId);

            await unitOfWork.TarefaRepository.AddAsync(tarefa);
            await unitOfWork.SaveChangesAsync();

            unitOfWork.TarefaRepository.Detatch(tarefa);

            RemoveTarefaCommand request = new()
            {
                Id = tarefaId
            };

            GetTarefaQuery request2 = new()
            {
                Id = tarefaId
            };

            // Act
            TarefaHandler handler = new(unitOfWork, mapper);
            RemoveTarefaResponse response = await handler.Handle(request, CancellationToken.None);
            GetTarefaResponse response2 = await handler.Handle(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.NotFound);
            Assert.True(response2.Tarefa == null);
        }

        [Fact]
        public async Task UpdateTarefaCommand_Handle()
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
            DateTime dataInclusao = DateTime.Now;
            Tarefa tarefa = MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId);

            await unitOfWork.TarefaRepository.AddAsync(tarefa);
            await unitOfWork.SaveChangesAsync();

            unitOfWork.TarefaRepository.Detatch(tarefa);

            UpdateTarefaCommand request = new()
            {
                Tarefa = MockViewModelHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId, dataInclusao)
            };

            GetTarefaQuery request2 = new()
            {
                Id = tarefaId
            };

            // Act
            TarefaHandler handler = new(unitOfWork, mapper);
            UpdateTarefaResponse response = await handler.Handle(request, CancellationToken.None);
            GetTarefaResponse response2 = await handler.Handle(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.Success);
            Assert.True(response2.Tarefa != null);
            Assert.True(response2.Tarefa.Id == tarefaId);
            Assert.True(response2.Tarefa.DataInclusao.Ticks == dataInclusao.Ticks);
        }
    }
}
