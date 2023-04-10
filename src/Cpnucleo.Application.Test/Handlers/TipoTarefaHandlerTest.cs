namespace Cpnucleo.Application.Test.Handlers;

public class TipoTarefaHandlerTest
{
    [Fact]
    public async Task CreateTipoTarefaCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        await DbContextHelper.SeedData(context);

        CreateTipoTarefaCommand request = MockCommandHelper.GetNewCreateTipoTarefaCommand();

        // Act
        CreateTipoTarefaCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetTipoTarefaQuery_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var tipoTarefa = context.TipoTarefas.First();

        GetTipoTarefaQuery request = MockQueryHelper.GetNewGetTipoTarefaQuery(tipoTarefa.Id);

        // Act
        GetTipoTarefaQueryHandler handler = new(context, mapper);
        GetTipoTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.TipoTarefa != null);
        Assert.True(response.TipoTarefa.Id != Guid.Empty);
        Assert.True(response.TipoTarefa.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListTipoTarefaQuery_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        ListTipoTarefaQuery request = MockQueryHelper.GetNewListTipoTarefaQuery();

        // Act
        ListTipoTarefaQueryHandler handler = new(context, mapper);
        ListTipoTarefaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.TipoTarefas != null);
        Assert.True(response.TipoTarefas.Any());
    }

    [Fact]
    public async Task RemoveTipoTarefaCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var tipoTarefa = context.TipoTarefas.First();

        RemoveTipoTarefaCommand request = MockCommandHelper.GetNewRemoveTipoTarefaCommand(tipoTarefa.Id);
        GetTipoTarefaQuery request2 = MockQueryHelper.GetNewGetTipoTarefaQuery();

        // Act
        RemoveTipoTarefaCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetTipoTarefaQueryHandler handler2 = new(context, mapper);
        GetTipoTarefaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateTipoTarefaCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var tipoTarefa = context.TipoTarefas.First();

        UpdateTipoTarefaCommand request = MockCommandHelper.GetNewUpdateTipoTarefaCommand(tipoTarefa.Id);
        GetTipoTarefaQuery request2 = MockQueryHelper.GetNewGetTipoTarefaQuery(tipoTarefa.Id);

        // Act
        UpdateTipoTarefaCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetTipoTarefaQueryHandler handler2 = new(context, mapper);
        GetTipoTarefaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.TipoTarefa != null);
        Assert.True(response2.TipoTarefa.Id == tipoTarefa.Id);
    }
}
