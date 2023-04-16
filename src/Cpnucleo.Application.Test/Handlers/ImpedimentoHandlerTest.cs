namespace Cpnucleo.Application.Test.Handlers;

public class ImpedimentoHandlerTest
{
    [Fact]
    public async Task CreateImpedimentoCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var request = MockCommandHelper.GetNewCreateImpedimentoCommand();

        // Act
        CreateImpedimentoCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetImpedimentoQuery_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var impedimento = context.Impedimentos.First();

        var request = MockQueryHelper.GetNewGetImpedimentoQuery(impedimento.Id);

        // Act
        GetImpedimentoQueryHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Impedimento != null);
        Assert.True(response.Impedimento.Id != Guid.Empty);
        Assert.True(response.Impedimento.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListImpedimentoQuery_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var request = MockQueryHelper.GetNewListImpedimentoQuery();

        // Act
        ListImpedimentoQueryHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Impedimentos != null);
        Assert.True(response.Impedimentos.Any());
    }

    [Fact]
    public async Task RemoveImpedimentoCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var impedimento = context.Impedimentos.First();

        var request = MockCommandHelper.GetNewRemoveImpedimentoCommand(impedimento.Id);
        var request2 = MockQueryHelper.GetNewGetImpedimentoQuery(impedimento.Id);

        // Act
        RemoveImpedimentoCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        GetImpedimentoQueryHandler handler2 = new(context);
        var response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateImpedimentoCommand_Handle_Success()
    {
        // Arrange
        var context = DbContextHelper.GetContext();

        var impedimento = context.Impedimentos.First();

        var request = MockCommandHelper.GetNewUpdateImpedimentoCommand(impedimento.Id);
        var request2 = MockQueryHelper.GetNewGetImpedimentoQuery(impedimento.Id);

        // Act
        UpdateImpedimentoCommandHandler handler = new(context);
        var response = await handler.Handle(request, CancellationToken.None);

        GetImpedimentoQueryHandler handler2 = new(context);
        var response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.Impedimento != null);
        Assert.True(response2.Impedimento.Id == impedimento.Id);
    }
}
