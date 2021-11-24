using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Application.Test.Handlers;

public class RecursoHandlerTest
{
    [Fact]
    public async Task CreateRecursoCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        ICryptographyManager manager = CryptographyHelper.GetInstance();

        CreateRecursoCommand request = new()
        {
            Recurso = MockViewModelHelper.GetNewRecurso()
        };

        // Act
        RecursoHandler handler = new(unitOfWork, mapper, manager);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetRecursoQuery_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        ICryptographyManager manager = CryptographyHelper.GetInstance();

        Guid recursoId = Guid.NewGuid();

        await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));
        await unitOfWork.SaveChangesAsync();

        GetRecursoQuery request = new()
        {
            Id = recursoId
        };

        // Act
        RecursoHandler handler = new(unitOfWork, mapper, manager);
        RecursoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Id != Guid.Empty);
        Assert.True(response.DataInclusao.Ticks != 0);
        Assert.True(response.Senha == null);
        Assert.True(response.Salt == null);
    }

    [Fact]
    public async Task ListRecursoQuery_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        ICryptographyManager manager = CryptographyHelper.GetInstance();

        Guid recursoId = Guid.NewGuid();

        await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));
        await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso());
        await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso());

        await unitOfWork.SaveChangesAsync();

        ListRecursoQuery request = new();

        // Act
        RecursoHandler handler = new(unitOfWork, mapper, manager);
        IEnumerable<RecursoViewModel> response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Any());
        Assert.True(response.FirstOrDefault(x => x.Id == recursoId) != null);
    }

    [Fact]
    public async Task RemoveRecursoCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        ICryptographyManager manager = CryptographyHelper.GetInstance();

        Guid recursoId = Guid.NewGuid();

        Recurso recurso = MockEntityHelper.GetNewRecurso(recursoId);

        await unitOfWork.RecursoRepository.AddAsync(recurso);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.RecursoRepository.Detatch(recurso);

        RemoveRecursoCommand request = new()
        {
            Id = recursoId
        };

        GetRecursoQuery request2 = new()
        {
            Id = recursoId
        };

        // Act
        RecursoHandler handler = new(unitOfWork, mapper, manager);
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        RecursoViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 == null);
    }

    [Fact]
    public async Task UpdateRecursoCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        ICryptographyManager manager = CryptographyHelper.GetInstance();

        Guid recursoId = Guid.NewGuid();
        DateTime dataInclusao = DateTime.Now;

        Recurso recurso = MockEntityHelper.GetNewRecurso(recursoId);

        await unitOfWork.RecursoRepository.AddAsync(recurso);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.RecursoRepository.Detatch(recurso);

        UpdateRecursoCommand request = new()
        {
            Recurso = MockViewModelHelper.GetNewRecurso(recursoId, dataInclusao)
        };

        GetRecursoQuery request2 = new()
        {
            Id = recursoId
        };

        // Act
        RecursoHandler handler = new(unitOfWork, mapper, manager);
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        RecursoViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 != null);
        Assert.True(response2.Id == recursoId);
        Assert.True(response2.DataInclusao.Ticks == dataInclusao.Ticks);
        Assert.True(response2.Senha == null);
        Assert.True(response2.Salt == null);
    }
}
