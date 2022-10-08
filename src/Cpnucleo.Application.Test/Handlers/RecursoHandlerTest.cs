using Cpnucleo.Application.Commands.Recurso;
using Cpnucleo.Domain.Common.Security.Interfaces;
using Cpnucleo.Application.Queries.Recurso;

namespace Cpnucleo.Application.Test.Handlers;

public class RecursoHandlerTest
{
    [Fact]
    public async Task CreateRecursoCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();
        ICryptographyManager manager = CryptographyHelper.GetInstance();

        CreateRecursoCommand request = MockCommandHelper.GetNewCreateRecursoCommand();

        // Act
        CreateRecursoHandler handler = new(unitOfWork, mapper, manager);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetRecursoQuery_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid recursoId = Guid.NewGuid();

        await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));
        await unitOfWork.SaveChangesAsync();

        GetRecursoQuery request = MockQueryHelper.GetNewGetRecursoQuery(recursoId);

        // Act
        GetRecursoHandler handler = new(unitOfWork, mapper);
        GetRecursoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Recurso != null);
        Assert.True(response.Recurso.Id != Guid.Empty);
        Assert.True(response.Recurso.DataInclusao.Ticks != 0);
        Assert.True(response.Recurso.Senha == null);
    }

    [Fact]
    public async Task ListRecursoQuery_Handle_Success()
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

        ListRecursoQuery request = MockQueryHelper.GetNewListRecursoQuery();

        // Act
        ListRecursoHandler handler = new(unitOfWork, mapper);
        ListRecursoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.Recursos != null);
        Assert.True(response.Recursos.Any());
        Assert.True(response.Recursos.FirstOrDefault(x => x.Id == recursoId) != null);
    }

    [Fact]
    public async Task RemoveRecursoCommand_Handle_Success()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid recursoId = Guid.NewGuid();

        Recurso recurso = MockEntityHelper.GetNewRecurso(recursoId);

        await unitOfWork.RecursoRepository.AddAsync(recurso);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.RecursoRepository.Detatch(recurso);

        RemoveRecursoCommand request = MockCommandHelper.GetNewRemoveRecursoCommand(recursoId);
        GetRecursoQuery request2 = MockQueryHelper.GetNewGetRecursoQuery(recursoId);

        // Act
        RemoveRecursoHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetRecursoHandler handler2 = new(unitOfWork, mapper);
        GetRecursoViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateRecursoCommand_Handle_Success()
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

        UpdateRecursoCommand request = MockCommandHelper.GetNewUpdateRecursoCommand(recursoId);
        GetRecursoQuery request2 = MockQueryHelper.GetNewGetRecursoQuery(recursoId);

        // Act
        UpdateRecursoHandler handler = new(unitOfWork, manager);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetRecursoHandler handler2 = new(unitOfWork, mapper);
        GetRecursoViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.Recurso != null);
        Assert.True(response2.Recurso.Id == recursoId);
        Assert.True(response2.Recurso.Senha == null);
    }
}
