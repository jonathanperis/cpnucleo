namespace Cpnucleo.Application.Test.Handlers;

public class TarefaHandlerTest
{
    [Fact]
    public async Task CreateTarefaCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();

        var projeto = context.Projetos.First();
        var workflow = context.Workflows.First();
        var recurso = context.Recursos.First();
        var tipoTarefa = context.TipoTarefas.First();

        CreateTarefaCommand request = MockCommandHelper.GetNewCreateTarefaCommand(projeto.Id, workflow.Id, recurso.Id, tipoTarefa.Id);

        // Act
        CreateTarefaCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetTarefaQuery_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();

        var tarefa = context.Tarefas.First();

        GetTarefaQuery request = MockQueryHelper.GetNewGetTarefaQuery(tarefa.Id);

        // Act
        GetTarefaQueryHandler handler = new(context);
        GetTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Tarefa != null);
        Assert.True(response.Tarefa.Id != Guid.Empty);
        Assert.True(response.Tarefa.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListTarefaQuery_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();

        ListTarefaQuery request = MockQueryHelper.GetNewListTarefaQuery();

        // Act
        ListTarefaQueryHandler handler = new(context);
        ListTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Tarefas != null);
        Assert.True(response.Tarefas.Any());
    }

    [Fact]
    public async Task RemoveTarefaCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();

        var tarefa = context.Tarefas.First();

        RemoveTarefaCommand request = MockCommandHelper.GetNewRemoveTarefaCommand(tarefa.Id);
        GetTarefaQuery request2 = MockQueryHelper.GetNewGetTarefaQuery(tarefa.Id);

        // Act
        RemoveTarefaCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetTarefaQueryHandler handler2 = new(context);
        GetTarefaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateTarefaCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        
        var projeto = context.Projetos.First();
        var workflow = context.Workflows.First();
        var recurso = context.Recursos.First();
        var tipoTarefa = context.TipoTarefas.First();
        var tarefa = context.Tarefas.First();

        UpdateTarefaCommand request = MockCommandHelper.GetNewUpdateTarefaCommand(projeto.Id, workflow.Id, recurso.Id, tipoTarefa.Id, tarefa.Id);
        GetTarefaQuery request2 = MockQueryHelper.GetNewGetTarefaQuery(tarefa.Id);

        // Act
        UpdateTarefaCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetTarefaQueryHandler handler2 = new(context);
        GetTarefaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.Tarefa != null);
        Assert.True(response2.Tarefa.Id == tarefa.Id);
    }

    [Fact]
    public async Task UpdateTarefaByWorkflowCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();

        var workflow = context.Workflows.First();
        var tarefa = context.Tarefas.First();

        UpdateTarefaByWorkflowCommand request = MockCommandHelper.GetNewUpdateTarefaByWorkflowCommand(workflow.Id, tarefa.Id);
        GetTarefaQuery request2 = MockQueryHelper.GetNewGetTarefaQuery(tarefa.Id);

        // Act
        UpdateTarefaByWorkflowCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetTarefaQueryHandler handler2 = new(context);
        GetTarefaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.Tarefa != null);
        Assert.True(response2.Tarefa.Id == tarefa.Id);
        Assert.True(response2.Tarefa.IdWorkflow == workflow.Id);
    }

    [Fact]
    public async Task ListTarefaByRecursoQuery_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();

        var recurso = context.Recursos.First();

        ListTarefaByRecursoQuery request = MockQueryHelper.GetNewListTarefaByRecursoQuery(recurso.Id);

        // Act
        ListTarefaByRecursoQueryHandler handler = new(context);
        ListTarefaByRecursoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Tarefas != null);
        Assert.True(response.Tarefas.Any());
    }
}
