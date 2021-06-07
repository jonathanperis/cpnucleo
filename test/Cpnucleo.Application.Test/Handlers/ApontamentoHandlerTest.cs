using AutoMapper;
using Cpnucleo.Application.Handlers;
using Cpnucleo.Application.Test.Helpers;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.UoW;
using Cpnucleo.Infra.CrossCutting.Util;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.CreateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.RemoveApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.UpdateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.ListApontamento;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cpnucleo.Application.Test.Handlers
{
    public class ApontamentoHandlerTest
    {
        [Fact]
        public async Task CreateApontamentoCommand_Handle()
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

            CreateApontamentoCommand request = new()
            {
                Apontamento = MockViewModelHelper.GetNewApontamento(tarefaId, recursoId)
            };

            // Act
            ApontamentoHandler handler = new(unitOfWork, mapper);
            CreateApontamentoResponse response = await handler.InvokeAsync(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.Apontamento != null);
            Assert.True(response.Apontamento.Id != Guid.Empty);
            Assert.True(response.Apontamento.DataInclusao.Ticks != 0);
        }

        [Fact]
        public async Task GetApontamentoQuery_Handle()
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

            Guid apontamentoId = Guid.NewGuid();
            await unitOfWork.ApontamentoRepository.AddAsync(MockEntityHelper.GetNewApontamento(tarefaId, recursoId, apontamentoId));

            await unitOfWork.SaveChangesAsync();

            GetApontamentoQuery request = new()
            {
                Id = apontamentoId
            };

            // Act
            ApontamentoHandler handler = new(unitOfWork, mapper);
            GetApontamentoResponse response = await handler.InvokeAsync(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.Apontamento != null);
            Assert.True(response.Apontamento.Id != Guid.Empty);
            Assert.True(response.Apontamento.DataInclusao.Ticks != 0);
        }

        [Fact]
        public async Task ListApontamentoQuery_Handle()
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

            Guid apontamentoId = Guid.NewGuid();
            await unitOfWork.ApontamentoRepository.AddAsync(MockEntityHelper.GetNewApontamento(tarefaId, recursoId, apontamentoId));
            await unitOfWork.ApontamentoRepository.AddAsync(MockEntityHelper.GetNewApontamento(tarefaId, recursoId));
            await unitOfWork.ApontamentoRepository.AddAsync(MockEntityHelper.GetNewApontamento(tarefaId, recursoId));

            await unitOfWork.SaveChangesAsync();

            ListApontamentoQuery request = new();

            // Act
            ApontamentoHandler handler = new(unitOfWork, mapper);
            ListApontamentoResponse response = await handler.InvokeAsync(request, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response.Apontamentos != null);
            Assert.True(response.Apontamentos.Any());
            Assert.True(response.Apontamentos.FirstOrDefault(x => x.Id == apontamentoId) != null);
        }

        [Fact]
        public async Task RemoveApontamentoCommand_Handle()
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

            Guid apontamentoId = Guid.NewGuid();
            Apontamento apontamento = MockEntityHelper.GetNewApontamento(tarefaId, recursoId, apontamentoId);

            await unitOfWork.ApontamentoRepository.AddAsync(apontamento);
            await unitOfWork.SaveChangesAsync();

            unitOfWork.ApontamentoRepository.Detatch(apontamento);

            RemoveApontamentoCommand request = new()
            {
                Id = apontamentoId
            };

            GetApontamentoQuery request2 = new()
            {
                Id = apontamentoId
            };

            // Act
            ApontamentoHandler handler = new(unitOfWork, mapper);
            RemoveApontamentoResponse response = await handler.InvokeAsync(request, CancellationToken.None);
            GetApontamentoResponse response2 = await handler.InvokeAsync(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.NotFound);
            Assert.True(response2.Apontamento == null);
        }

        [Fact]
        public async Task UpdateApontamentoCommand_Handle()
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

            Guid apontamentoId = Guid.NewGuid();
            DateTime dataInclusao = DateTime.Now;
            Apontamento apontamento = MockEntityHelper.GetNewApontamento(tarefaId, recursoId, apontamentoId);

            await unitOfWork.ApontamentoRepository.AddAsync(apontamento);
            await unitOfWork.SaveChangesAsync();

            unitOfWork.ApontamentoRepository.Detatch(apontamento);

            UpdateApontamentoCommand request = new()
            {
                Apontamento = MockViewModelHelper.GetNewApontamento(tarefaId, recursoId, apontamentoId, dataInclusao)
            };

            GetApontamentoQuery request2 = new()
            {
                Id = apontamentoId
            };

            // Act
            ApontamentoHandler handler = new(unitOfWork, mapper);
            UpdateApontamentoResponse response = await handler.InvokeAsync(request, CancellationToken.None);
            GetApontamentoResponse response2 = await handler.InvokeAsync(request2, CancellationToken.None);

            // Assert
            Assert.True(response.Status == OperationResult.Success);
            Assert.True(response2.Status == OperationResult.Success);
            Assert.True(response2.Apontamento != null);
            Assert.True(response2.Apontamento.Id == apontamentoId);
            Assert.True(response2.Apontamento.DataInclusao.Ticks == dataInclusao.Ticks);
        }
    }
}
