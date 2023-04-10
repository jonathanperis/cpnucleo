namespace Cpnucleo.Application.Test.Handlers;

public class SistemaHandlerTest
{
    [Fact]
    public async Task CreateSistemaCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        await DbContextHelper.SeedData(context);

        CreateSistemaCommand request = MockCommandHelper.GetNewCreateSistemaCommand();

        // Act
        CreateSistemaCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetSistemaQuery_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var sistema = context.Sistemas.First();

        GetSistemaQuery request = MockQueryHelper.GetNewGetSistemaQuery(sistema.Id);

        // Act
        GetSistemaQueryHandler handler = new(context, mapper);
        GetSistemaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Sistema != null);
        Assert.True(response.Sistema.Id != Guid.Empty);
        Assert.True(response.Sistema.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListSistemaQuery_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        IHubContext<ApplicationHub> mockSignalR = SignalRHelper.GetInstance();
        await DbContextHelper.SeedData(context);

        ListSistemaQuery request = MockQueryHelper.GetNewListSistemaQuery();

        // Act
        ListSistemaQueryHandler handler = new(context, mapper, mockSignalR);
        ListSistemaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Sistemas != null);
        Assert.True(response.Sistemas.Any());
    }

    [Fact]
    public async Task RemoveSistemaCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);
        
        var sistema = context.Sistemas.First();
        
        IEventManager eventManager = EventManagerHelper.GetInstance(new RemoveSistemaEvent(sistema.Id));

        RemoveSistemaCommand request = MockCommandHelper.GetNewRemoveSistemaCommand(sistema.Id);
        GetSistemaQuery request2 = MockQueryHelper.GetNewGetSistemaQuery();

        // Act
        RemoveSistemaCommandHandler handler = new(context, eventManager);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetSistemaQueryHandler handler2 = new(context, mapper);
        GetSistemaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateSistemaCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var sistema = context.Sistemas.First();

        UpdateSistemaCommand request = MockCommandHelper.GetNewUpdateSistemaCommand(sistema.Id);
        GetSistemaQuery request2 = MockQueryHelper.GetNewGetSistemaQuery(sistema.Id);

        // Act
        UpdateSistemaCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetSistemaQueryHandler handler2 = new(context, mapper);
        GetSistemaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.Sistema != null);
        Assert.True(response2.Sistema.Id == sistema.Id);
    }
}