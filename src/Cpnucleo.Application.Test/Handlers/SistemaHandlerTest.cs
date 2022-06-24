using Cpnucleo.Application.Commands.Sistema;
using Cpnucleo.Application.Hubs;
using Cpnucleo.Application.Queries.Sistema;
using Cpnucleo.Infra.CrossCutting.Bus.Events.Sistema;
using Cpnucleo.Infrastructure.Bus.Interfaces;
using Cpnucleo.Shared.Commands.Sistema;
using Cpnucleo.Shared.Common.Models;
using Cpnucleo.Shared.Queries.Sistema;
using Microsoft.AspNetCore.SignalR;
using Moq;

namespace Cpnucleo.Application.Test.Handlers;

public class SistemaHandlerTest
{
    [Fact]
    public async Task CreateSistemaCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        CreateSistemaCommand request = MockCommandHelper.GetNewCreateSistemaCommand();

        // Act
        CreateSistemaHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    //[Theory]
    //[InlineData("Sistema de teste", "")]
    //[InlineData("", "Descrição do sistema de teste")]
    //public async Task CreateSistemaCommand_Handle_Fail(string nome, string descricao)
    //{
    //    // Arrange
    //    IUnitOfWork unitOfWork = DbContextHelper.GetContext();
    //    IMapper mapper = AutoMapperHelper.GetMappings();

    //    CreateSistemaCommand command = MockCommandHelper.GetNewCreateSistemaCommand();

    //    command.Nome = nome;
    //    command.Descricao = descricao;

    //    // Act
    //    CreateSistemaHandler handler = new(unitOfWork, mapper);
    //    OperationResult response = await handler.Handle(command, CancellationToken.None);

    //    // Assert
    //    Assert.True(response == OperationResult.Failed);
    //}

    [Fact]
    public async Task GetSistemaQuery_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();

        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));
        await unitOfWork.SaveChangesAsync();

        GetSistemaQuery request = MockQueryHelper.GetNewGetSistemaQuery(sistemaId);

        // Act
        GetSistemaHandler handler = new(unitOfWork, mapper);
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
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Mock<IHubContext<CpnucleoHub>> mockSignalR = SignalRHelper.GetInstance();

        Guid sistemaId = Guid.NewGuid();

        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));
        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema());
        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema());

        await unitOfWork.SaveChangesAsync();

        ListSistemaQuery request = MockQueryHelper.GetNewListSistemaQuery();

        // Act
        ListSistemaHandler handler = new(unitOfWork, mapper, mockSignalR.Object);
        ListSistemaViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Sistemas != null);
        Assert.True(response.Sistemas.Any());
        Assert.True(response.Sistemas.FirstOrDefault(x => x.Id == sistemaId) != null);
    }

    [Fact]
    public async Task RemoveSistemaCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();

        Mock<IEventHandler> mockServiceBus = ServiceBusHelper.GetInstance(new RemoveSistemaEvent { Id = sistemaId });

        Sistema sistema = MockEntityHelper.GetNewSistema(sistemaId);

        await unitOfWork.SistemaRepository.AddAsync(sistema);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.SistemaRepository.Detatch(sistema);

        RemoveSistemaCommand request = MockCommandHelper.GetNewRemoveSistemaCommand(sistemaId);
        GetSistemaQuery request2 = MockQueryHelper.GetNewGetSistemaQuery(sistemaId);

        // Act
        RemoveSistemaHandler handler = new(unitOfWork, mockServiceBus.Object);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetSistemaHandler handler2 = new(unitOfWork, mapper);
        GetSistemaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateSistemaCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();

        Sistema sistema = MockEntityHelper.GetNewSistema(sistemaId);

        await unitOfWork.SistemaRepository.AddAsync(sistema);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.SistemaRepository.Detatch(sistema);

        UpdateSistemaCommand request = MockCommandHelper.GetNewUpdateSistemaCommand(sistemaId);
        GetSistemaQuery request2 = MockQueryHelper.GetNewGetSistemaQuery(sistemaId);

        // Act
        UpdateSistemaHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetSistemaHandler handler2 = new(unitOfWork, mapper);
        GetSistemaViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.Sistema != null);
        Assert.True(response2.Sistema.Id == sistemaId);
    }
}
