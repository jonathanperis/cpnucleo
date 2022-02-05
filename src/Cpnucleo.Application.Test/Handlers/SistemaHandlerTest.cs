using Cpnucleo.Application.Hubs;
using Cpnucleo.Infra.CrossCutting.Bus.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Events.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace Cpnucleo.Application.Test.Handlers;

public class SistemaHandlerTest
{
    [Fact]
    public async Task CreateSistemaCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Mock<IEventHandler> mockServiceBus = ServiceBusHelper.GetInstance();
        Mock<IHubContext<CpnucleoHub>> mockSignalR = SignalRHelper.GetInstance();

        CreateSistemaCommand request = new()
        {
            Sistema = MockViewModelHelper.GetNewSistema()
        };

        // Act
        SistemaHandler handler = new(unitOfWork, mapper, mockServiceBus.Object, mockSignalR.Object);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetSistemaQuery_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Mock<IEventHandler> mockServiceBus = ServiceBusHelper.GetInstance();
        Mock<IHubContext<CpnucleoHub>> mockSignalR = SignalRHelper.GetInstance();

        Guid sistemaId = Guid.NewGuid();

        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));
        await unitOfWork.SaveChangesAsync();

        GetSistemaQuery request = new()
        {
            Id = sistemaId
        };

        // Act
        SistemaHandler handler = new(unitOfWork, mapper, mockServiceBus.Object, mockSignalR.Object);
        SistemaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Id != Guid.Empty);
        Assert.True(response.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListSistemaQuery_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Mock<IEventHandler> mockServiceBus = ServiceBusHelper.GetInstance();
        Mock<IHubContext<CpnucleoHub>> mockSignalR = SignalRHelper.GetInstance();

        Guid sistemaId = Guid.NewGuid();

        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));
        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema());
        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema());

        await unitOfWork.SaveChangesAsync();

        ListSistemaQuery request = new();

        // Act
        SistemaHandler handler = new(unitOfWork, mapper, mockServiceBus.Object, mockSignalR.Object);
        IEnumerable<SistemaViewModel> response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Any());
        Assert.True(response.FirstOrDefault(x => x.Id == sistemaId) != null);
    }

    [Fact]
    public async Task RemoveSistemaCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();

        Mock<IEventHandler> mockServiceBus = ServiceBusHelper.GetInstance(new RemoveSistemaEvent { Id = sistemaId });
        Mock<IHubContext<CpnucleoHub>> mockSignalR = SignalRHelper.GetInstance();

        Sistema sistema = MockEntityHelper.GetNewSistema(sistemaId);

        await unitOfWork.SistemaRepository.AddAsync(sistema);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.SistemaRepository.Detatch(sistema);

        RemoveSistemaCommand request = new()
        {
            Id = sistemaId
        };

        GetSistemaQuery request2 = new()
        {
            Id = sistemaId
        };

        // Act
        SistemaHandler handler = new(unitOfWork, mapper, mockServiceBus.Object, mockSignalR.Object);
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        SistemaViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 == null);
    }

    [Fact]
    public async Task UpdateSistemaCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Mock<IEventHandler> mockServiceBus = ServiceBusHelper.GetInstance();
        Mock<IHubContext<CpnucleoHub>> mockSignalR = SignalRHelper.GetInstance();

        Guid sistemaId = Guid.NewGuid();
        DateTime dataInclusao = DateTime.Now;

        Sistema sistema = MockEntityHelper.GetNewSistema(sistemaId);

        await unitOfWork.SistemaRepository.AddAsync(sistema);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.SistemaRepository.Detatch(sistema);

        UpdateSistemaCommand request = new()
        {
            Sistema = MockViewModelHelper.GetNewSistema(sistemaId, dataInclusao)
        };

        GetSistemaQuery request2 = new()
        {
            Id = sistemaId
        };

        // Act
        SistemaHandler handler = new(unitOfWork, mapper, mockServiceBus.Object, mockSignalR.Object);
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        SistemaViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 != null);
        Assert.True(response2.Id == sistemaId);
        Assert.True(response2.DataInclusao.Ticks == dataInclusao.Ticks);
    }
}
