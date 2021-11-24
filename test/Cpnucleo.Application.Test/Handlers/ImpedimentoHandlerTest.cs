using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Application.Test.Handlers;

public class ImpedimentoHandlerTest
{
    [Fact]
    public async Task CreateImpedimentoCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        CreateImpedimentoCommand request = new()
        {
            Impedimento = MockViewModelHelper.GetNewImpedimento()
        };

        // Act
        ImpedimentoHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetImpedimentoQuery_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid impedimentoId = Guid.NewGuid();

        await unitOfWork.ImpedimentoRepository.AddAsync(MockEntityHelper.GetNewImpedimento(impedimentoId));
        await unitOfWork.SaveChangesAsync();

        GetImpedimentoQuery request = new()
        {
            Id = impedimentoId
        };

        // Act
        ImpedimentoHandler handler = new(unitOfWork, mapper);
        ImpedimentoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Id != Guid.Empty);
        Assert.True(response.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListImpedimentoQuery_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid impedimentoId = Guid.NewGuid();

        await unitOfWork.ImpedimentoRepository.AddAsync(MockEntityHelper.GetNewImpedimento(impedimentoId));
        await unitOfWork.ImpedimentoRepository.AddAsync(MockEntityHelper.GetNewImpedimento());
        await unitOfWork.ImpedimentoRepository.AddAsync(MockEntityHelper.GetNewImpedimento());

        await unitOfWork.SaveChangesAsync();

        ListImpedimentoQuery request = new();

        // Act
        ImpedimentoHandler handler = new(unitOfWork, mapper);
        IEnumerable<ImpedimentoViewModel> response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Any());
        Assert.True(response.FirstOrDefault(x => x.Id == impedimentoId) != null);
    }

    [Fact]
    public async Task RemoveImpedimentoCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid impedimentoId = Guid.NewGuid();

        Impedimento impedimento = MockEntityHelper.GetNewImpedimento(impedimentoId);

        await unitOfWork.ImpedimentoRepository.AddAsync(impedimento);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.ImpedimentoRepository.Detatch(impedimento);

        RemoveImpedimentoCommand request = new()
        {
            Id = impedimentoId
        };

        GetImpedimentoQuery request2 = new()
        {
            Id = impedimentoId
        };

        // Act
        ImpedimentoHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        ImpedimentoViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 == null);
    }

    [Fact]
    public async Task UpdateImpedimentoCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid impedimentoId = Guid.NewGuid();
        DateTime dataInclusao = DateTime.Now;

        Impedimento impedimento = MockEntityHelper.GetNewImpedimento(impedimentoId);

        await unitOfWork.ImpedimentoRepository.AddAsync(impedimento);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.ImpedimentoRepository.Detatch(impedimento);

        UpdateImpedimentoCommand request = new()
        {
            Impedimento = MockViewModelHelper.GetNewImpedimento(impedimentoId, dataInclusao)
        };

        GetImpedimentoQuery request2 = new()
        {
            Id = impedimentoId
        };

        // Act
        ImpedimentoHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        ImpedimentoViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 != null);
        Assert.True(response2.Id == impedimentoId);
        Assert.True(response2.DataInclusao.Ticks == dataInclusao.Ticks);
    }
}
