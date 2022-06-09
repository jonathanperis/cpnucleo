using Cpnucleo.Application.Commands.Impedimento;
using Cpnucleo.Application.Queries.Impedimento;

namespace Cpnucleo.Application.Test.Handlers;

public class ImpedimentoHandlerTest
{
    [Fact]
    public async Task CreateImpedimentoCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        CreateImpedimentoCommand request = MockCommandHelper.GetNewCreateImpedimentoCommand();

        // Act
        CreateImpedimentoHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetImpedimentoQuery_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid impedimentoId = Guid.NewGuid();

        await unitOfWork.ImpedimentoRepository.AddAsync(MockEntityHelper.GetNewImpedimento(impedimentoId));
        await unitOfWork.SaveChangesAsync();

        GetImpedimentoQuery request = MockQueryHelper.GetNewGetImpedimentoQuery(impedimentoId);
            
        // Act
        GetImpedimentoHandler handler = new(unitOfWork, mapper);
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
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid impedimentoId = Guid.NewGuid();

        await unitOfWork.ImpedimentoRepository.AddAsync(MockEntityHelper.GetNewImpedimento(impedimentoId));
        await unitOfWork.ImpedimentoRepository.AddAsync(MockEntityHelper.GetNewImpedimento());
        await unitOfWork.ImpedimentoRepository.AddAsync(MockEntityHelper.GetNewImpedimento());

        await unitOfWork.SaveChangesAsync();

        ListImpedimentoQuery request = MockQueryHelper.GetNewListImpedimentoQuery();

        // Act
        ListImpedimentoHandler handler = new(unitOfWork, mapper);
        ListImpedimentoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Impedimentos != null);
        Assert.True(response.Impedimentos.Any());
        Assert.True(response.Impedimentos.FirstOrDefault(x => x.Id == impedimentoId) != null);
    }

    [Fact]
    public async Task RemoveImpedimentoCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid impedimentoId = Guid.NewGuid();

        Impedimento impedimento = MockEntityHelper.GetNewImpedimento(impedimentoId);

        await unitOfWork.ImpedimentoRepository.AddAsync(impedimento);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.ImpedimentoRepository.Detatch(impedimento);

        RemoveImpedimentoCommand request = MockCommandHelper.GetNewRemoveImpedimentoCommand(impedimentoId);
        GetImpedimentoQuery request2 = MockQueryHelper.GetNewGetImpedimentoQuery(impedimentoId);

        // Act
        RemoveImpedimentoHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetImpedimentoHandler handler2 = new(unitOfWork, mapper);
        GetImpedimentoViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateImpedimentoCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid impedimentoId = Guid.NewGuid();

        Impedimento impedimento = MockEntityHelper.GetNewImpedimento(impedimentoId);

        await unitOfWork.ImpedimentoRepository.AddAsync(impedimento);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.ImpedimentoRepository.Detatch(impedimento);

        UpdateImpedimentoCommand request = MockCommandHelper.GetNewUpdateImpedimentoCommand(impedimentoId);
        GetImpedimentoQuery request2 = MockQueryHelper.GetNewGetImpedimentoQuery(impedimentoId);

        // Act
        UpdateImpedimentoHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetImpedimentoHandler handler2 = new(unitOfWork, mapper);
        GetImpedimentoViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.Impedimento != null);
        Assert.True(response2.Impedimento.Id == impedimentoId);
    }
}
