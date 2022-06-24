using Cpnucleo.Application.Commands.RecursoTarefa;
using Cpnucleo.Application.Queries.RecursoTarefa;
using Cpnucleo.Shared.Commands.RecursoTarefa;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.RecursoTarefa;

namespace Cpnucleo.Application.Test.Handlers;

public class RecursoTarefaHandlerTest
{
    [Fact]
    public async Task CreateRecursoTarefaCommand_Handle_Success()
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

        CreateRecursoTarefaCommand request = MockCommandHelper.GetNewCreateRecursoTarefaCommand(tarefaId, recursoId);

        // Act
        CreateRecursoTarefaHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetRecursoTarefaQuery_Handle_Success()
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

        GetRecursoTarefaQuery request = MockQueryHelper.GetNewGetRecursoTarefaQuery(recursoTarefaId);

        // Act
        GetRecursoTarefaHandler handler = new(unitOfWork, mapper);
        GetRecursoTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.RecursoTarefa != null);
        Assert.True(response.RecursoTarefa.Id != Guid.Empty);
        Assert.True(response.RecursoTarefa.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task GetRecursoTarefaByTarefaQuery_Handle_Success()
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

        GetRecursoTarefaByTarefaQuery request = MockQueryHelper.GetNewGetRecursoTarefaByTarefaQuery(tarefaId);

        // Act
        GetRecursoTarefaByTarefaHandler handler = new(unitOfWork, mapper);
        GetRecursoTarefaByTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.RecursoTarefas != null);
        Assert.True(response.RecursoTarefas.Any());
        Assert.True(response.RecursoTarefas.FirstOrDefault(x => x.Id == recursoTarefaId) != null);
    }

    [Fact]
    public async Task ListRecursoTarefaQuery_Handle_Success()
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

        ListRecursoTarefaQuery request = MockQueryHelper.GetNewListRecursoTarefaQuery();

        // Act
        ListRecursoTarefaHandler handler = new(unitOfWork, mapper);
        ListRecursoTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.RecursoTarefas != null);
        Assert.True(response.RecursoTarefas.Any());
        Assert.True(response.RecursoTarefas.FirstOrDefault(x => x.Id == recursoTarefaId) != null);
    }

    [Fact]
    public async Task RemoveRecursoTarefaCommand_Handle_Success()
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

        RemoveRecursoTarefaCommand request = MockCommandHelper.GetNewRemoveRecursoTarefaCommand(recursoTarefaId);
        GetRecursoTarefaQuery request2 = MockQueryHelper.GetNewGetRecursoTarefaQuery(recursoTarefaId);

        // Act
        RemoveRecursoTarefaHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetRecursoTarefaHandler handler2 = new(unitOfWork, mapper);
        GetRecursoTarefaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateRecursoTarefaCommand_Handle_Success()
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

        UpdateRecursoTarefaCommand request = MockCommandHelper.GetNewUpdateRecursoTarefaCommand(tarefaId, recursoId, recursoTarefaId);
        GetRecursoTarefaQuery request2 = MockQueryHelper.GetNewGetRecursoTarefaQuery(recursoTarefaId);

        // Act
        UpdateRecursoTarefaHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetRecursoTarefaHandler handler2 = new(unitOfWork, mapper);
        GetRecursoTarefaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.RecursoTarefa != null);
        Assert.True(response2.RecursoTarefa.Id == recursoTarefaId);
    }
}
