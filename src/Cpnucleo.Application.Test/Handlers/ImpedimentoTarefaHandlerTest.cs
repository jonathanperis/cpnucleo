namespace Cpnucleo.Application.Test.Handlers;

public class ImpedimentoTarefaHandlerTest
{
    [Fact]
    public async Task CreateImpedimentoTarefaCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        await DbContextHelper.SeedData(context);

        var tarefa = context.Tarefas.First();
        var impedimento = context.Impedimentos.First();

        CreateImpedimentoTarefaCommand request = MockCommandHelper.GetNewCreateImpedimentoTarefaCommand(tarefa.Id, impedimento.Id);

        // Act
        CreateImpedimentoTarefaCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetImpedimentoTarefaQuery_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var impedimentoTarefa = context.ImpedimentoTarefas.First();

        GetImpedimentoTarefaQuery request = MockQueryHelper.GetNewGetImpedimentoTarefaQuery(impedimentoTarefa.Id);

        // Act
        GetImpedimentoTarefaQueryHandler handler = new(context, mapper);
        GetImpedimentoTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.ImpedimentoTarefa != null);
        Assert.True(response.ImpedimentoTarefa.Id != Guid.Empty);
        Assert.True(response.ImpedimentoTarefa.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListImpedimentoTarefaQuery_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        ListImpedimentoTarefaQuery request = MockQueryHelper.GetNewListImpedimentoTarefaQuery();

        // Act
        ListImpedimentoTarefaQueryHandler handler = new(context, mapper);
        ListImpedimentoTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.ImpedimentoTarefas != null);
        Assert.True(response.ImpedimentoTarefas.Any());
    }

    [Fact]
    public async Task RemoveImpedimentoTarefaCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var impedimentoTarefa = context.ImpedimentoTarefas.First();

        RemoveImpedimentoTarefaCommand request = MockCommandHelper.GetNewRemoveImpedimentoTarefaCommand(impedimentoTarefa.Id);
        GetImpedimentoTarefaQuery request2 = MockQueryHelper.GetNewGetImpedimentoTarefaQuery(impedimentoTarefa.Id);

        // Act
        RemoveImpedimentoTarefaCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetImpedimentoTarefaQueryHandler handler2 = new(context, mapper);
        GetImpedimentoTarefaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateImpedimentoTarefaCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var tarefa = context.Tarefas.First();
        var impedimento = context.Impedimentos.First();
        var impedimentoTarefa = context.ImpedimentoTarefas.First();

        UpdateImpedimentoTarefaCommand request = MockCommandHelper.GetNewUpdateImpedimentoTarefaCommand(tarefa.Id, impedimento.Id, impedimentoTarefa.Id);
        GetImpedimentoTarefaQuery request2 = MockQueryHelper.GetNewGetImpedimentoTarefaQuery(impedimentoTarefa.Id);

        // Act
        UpdateImpedimentoTarefaCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetImpedimentoTarefaQueryHandler handler2 = new(context, mapper);
        GetImpedimentoTarefaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.ImpedimentoTarefa != null);
        Assert.True(response2.ImpedimentoTarefa.Id == impedimentoTarefa.Id);
    }
}
