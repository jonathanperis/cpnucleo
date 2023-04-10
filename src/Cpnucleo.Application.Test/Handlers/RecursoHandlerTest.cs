namespace Cpnucleo.Application.Test.Handlers;

public class RecursoHandlerTest
{
    [Fact]
    public async Task CreateRecursoCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        await DbContextHelper.SeedData(context);

        CreateRecursoCommand request = MockCommandHelper.GetNewCreateRecursoCommand();

        // Act
        CreateRecursoCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetRecursoQuery_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var recurso = context.Recursos.First();

        GetRecursoQuery request = MockQueryHelper.GetNewGetRecursoQuery(recurso.Id);

        // Act
        GetRecursoQueryHandler handler = new(context, mapper);
        GetRecursoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Recurso != null);
        Assert.True(response.Recurso.Id != Guid.Empty);
        Assert.True(response.Recurso.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListRecursoQuery_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        ListRecursoQuery request = MockQueryHelper.GetNewListRecursoQuery();

        // Act
        ListRecursoQueryHandler handler = new(context, mapper);
        ListRecursoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Recursos != null);
        Assert.True(response.Recursos.Any());
    }

    [Fact]
    public async Task RemoveRecursoCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var recurso = context.Recursos.First();

        RemoveRecursoCommand request = MockCommandHelper.GetNewRemoveRecursoCommand(recurso.Id);
        GetRecursoQuery request2 = MockQueryHelper.GetNewGetRecursoQuery();

        // Act
        RemoveRecursoCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetRecursoQueryHandler handler2 = new(context, mapper);
        GetRecursoViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateRecursoCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var recurso = context.Recursos.First();

        UpdateRecursoCommand request = MockCommandHelper.GetNewUpdateRecursoCommand(recurso.Id);
        GetRecursoQuery request2 = MockQueryHelper.GetNewGetRecursoQuery(recurso.Id);

        // Act
        UpdateRecursoCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetRecursoQueryHandler handler2 = new(context, mapper);
        GetRecursoViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.Recurso != null);
        Assert.True(response2.Recurso.Id == recurso.Id);
    }
}
