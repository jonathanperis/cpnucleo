using Cpnucleo.Application.Commands.Tarefa;
using Cpnucleo.Application.Queries.Tarefa;

namespace Cpnucleo.Application.Test.Handlers;

public class TarefaHandlerTest
{
    [Fact]
    public async Task CreateTarefaCommand_Handle_Success()
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

        CreateTarefaCommand request = MockCommandHelper.GetNewCreateTarefaCommand(projetoId, workflowId, recursoId, tipoTarefaId);

        // Act
        CreateTarefaHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetTarefaQuery_Handle_Success()
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

        GetTarefaQuery request = MockQueryHelper.GetNewGetTarefaQuery(tarefaId);

        // Act
        GetTarefaHandler handler = new(unitOfWork, mapper);
        GetTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Tarefa != null);
        Assert.True(response.Tarefa.Id != Guid.Empty);
        Assert.True(response.Tarefa.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task GetTarefaByRecursoQuery_Handle_Success()
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

        GetTarefaByRecursoQuery request = MockQueryHelper.GetNewGetTarefaByRecursoQuery(recursoId);

        // Act
        GetTarefaByRecursoHandler handler = new(unitOfWork, mapper);
        GetTarefaByRecursoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Tarefas != null);
        Assert.True(response.Tarefas.Any());
        Assert.True(response.Tarefas.FirstOrDefault(x => x.Id == tarefaId) != null);
    }

    [Fact]
    public async Task ListTarefaQuery_Handle_Success()
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

        ListTarefaQuery request = MockQueryHelper.GetNewListTarefaQuery();

        // Act
        ListTarefaHandler handler = new(unitOfWork, mapper);
        ListTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Tarefas != null);
        Assert.True(response.Tarefas.Any());
        Assert.True(response.Tarefas.FirstOrDefault(x => x.Id == tarefaId) != null);
    }

    [Fact]
    public async Task RemoveTarefaCommand_Handle_Success()
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

        RemoveTarefaCommand request = MockCommandHelper.GetNewRemoveTarefaCommand(tarefaId);
        GetTarefaQuery request2 = MockQueryHelper.GetNewGetTarefaQuery(tarefaId);

        // Act
        RemoveTarefaHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetTarefaHandler handler2 = new(unitOfWork, mapper);
        GetTarefaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateTarefaCommand_Handle_Success()
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

        UpdateTarefaCommand request = MockCommandHelper.GetNewUpdateTarefaCommand(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId);
        GetTarefaQuery request2 = MockQueryHelper.GetNewGetTarefaQuery(tarefaId);

        // Act
        UpdateTarefaHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetTarefaHandler handler2 = new(unitOfWork, mapper);
        GetTarefaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.Tarefa != null);
        Assert.True(response2.Tarefa.Id == tarefaId);
    }

    [Fact]
    public async Task UpdateTarefaByWorkflowCommand_Handle_Success()
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

        Guid workflowId2 = Guid.NewGuid();
        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId2));

        Guid recursoId = Guid.NewGuid();
        await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));

        Guid tipoTarefaId = Guid.NewGuid();
        await unitOfWork.TipoTarefaRepository.AddAsync(MockEntityHelper.GetNewTipoTarefa(tipoTarefaId));

        Guid tarefaId = Guid.NewGuid();
        Tarefa tarefa = MockEntityHelper.GetNewTarefa(projetoId, workflowId, recursoId, tipoTarefaId, tarefaId);

        await unitOfWork.TarefaRepository.AddAsync(tarefa);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.TarefaRepository.Detatch(tarefa);

        UpdateTarefaByWorkflowCommand request = MockCommandHelper.GetNewUpdateTarefaByWorkflowCommand(workflowId2, tarefaId);
        GetTarefaQuery request2 = MockQueryHelper.GetNewGetTarefaQuery(tarefaId);

        // Act
        UpdateTarefaByWorkflowHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetTarefaHandler handler2 = new(unitOfWork, mapper);
        GetTarefaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.Tarefa != null);
        Assert.True(response2.Tarefa.Id == tarefaId);
        Assert.True(response2.Tarefa.IdWorkflow != workflowId);
    }
}
