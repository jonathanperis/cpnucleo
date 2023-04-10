﻿namespace Cpnucleo.Application.Test.Handlers;

public class ImpedimentoHandlerTest
{
    [Fact]
    public async Task CreateImpedimentoCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        await DbContextHelper.SeedData(context);

        CreateImpedimentoCommand request = MockCommandHelper.GetNewCreateImpedimentoCommand();

        // Act
        CreateImpedimentoCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetImpedimentoQuery_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var impedimento = context.Impedimentos.First();

        GetImpedimentoQuery request = MockQueryHelper.GetNewGetImpedimentoQuery(impedimento.Id);

        // Act
        GetImpedimentoQueryHandler handler = new(context, mapper);
        GetImpedimentoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Impedimento != null);
        Assert.True(response.Impedimento.Id != Guid.Empty);
        Assert.True(response.Impedimento.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListImpedimentoQuery_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        ListImpedimentoQuery request = MockQueryHelper.GetNewListImpedimentoQuery();

        // Act
        ListImpedimentoQueryHandler handler = new(context, mapper);
        ListImpedimentoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Impedimentos != null);
        Assert.True(response.Impedimentos.Any());
    }

    [Fact]
    public async Task RemoveImpedimentoCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var impedimento = context.Impedimentos.First();

        RemoveImpedimentoCommand request = MockCommandHelper.GetNewRemoveImpedimentoCommand(impedimento.Id);
        GetImpedimentoQuery request2 = MockQueryHelper.GetNewGetImpedimentoQuery();

        // Act
        RemoveImpedimentoCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetImpedimentoQueryHandler handler2 = new(context, mapper);
        GetImpedimentoViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateImpedimentoCommand_Handle_Success()
    {
        // Arrange
        IApplicationDbContext context = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        await DbContextHelper.SeedData(context);

        var impedimento = context.Impedimentos.First();

        UpdateImpedimentoCommand request = MockCommandHelper.GetNewUpdateImpedimentoCommand(impedimento.Id);
        GetImpedimentoQuery request2 = MockQueryHelper.GetNewGetImpedimentoQuery(impedimento.Id);

        // Act
        UpdateImpedimentoCommandHandler handler = new(context);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetImpedimentoQueryHandler handler2 = new(context, mapper);
        GetImpedimentoViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.Impedimento != null);
        Assert.True(response2.Impedimento.Id == impedimento.Id);
    }
}
