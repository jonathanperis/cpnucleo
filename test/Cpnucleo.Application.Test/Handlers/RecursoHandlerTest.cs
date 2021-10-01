using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.CreateRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.RemoveRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.UpdateRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.GetRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.ListRecurso;

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
        CreateRecursoResponse response = await handler.InvokeAsync(request, CancellationToken.None);

        // Assert
        Assert.True(response.Status == OperationResult.Success);
        Assert.True(response.Recurso != null);
        Assert.True(response.Recurso.Id != Guid.Empty);
        Assert.True(response.Recurso.DataInclusao.Ticks != 0);
        Assert.True(response.Recurso.Senha == null);
        Assert.True(response.Recurso.Salt == null);
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
        GetRecursoResponse response = await handler.InvokeAsync(request, CancellationToken.None);

        // Assert
        Assert.True(response.Status == OperationResult.Success);
        Assert.True(response.Recurso != null);
        Assert.True(response.Recurso.Id != Guid.Empty);
        Assert.True(response.Recurso.DataInclusao.Ticks != 0);
        Assert.True(response.Recurso.Senha == null);
        Assert.True(response.Recurso.Salt == null);
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
        ListRecursoResponse response = await handler.InvokeAsync(request, CancellationToken.None);

        // Assert
        Assert.True(response.Status == OperationResult.Success);
        Assert.True(response.Recursos != null);
        Assert.True(response.Recursos.Any());
        Assert.True(response.Recursos.FirstOrDefault(x => x.Id == recursoId) != null);
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
        RemoveRecursoResponse response = await handler.InvokeAsync(request, CancellationToken.None);
        GetRecursoResponse response2 = await handler.InvokeAsync(request2, CancellationToken.None);

        // Assert
        Assert.True(response.Status == OperationResult.Success);
        Assert.True(response2.Status == OperationResult.NotFound);
        Assert.True(response2.Recurso == null);
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
        UpdateRecursoResponse response = await handler.InvokeAsync(request, CancellationToken.None);
        GetRecursoResponse response2 = await handler.InvokeAsync(request2, CancellationToken.None);

        // Assert
        Assert.True(response.Status == OperationResult.Success);
        Assert.True(response2.Status == OperationResult.Success);
        Assert.True(response2.Recurso != null);
        Assert.True(response2.Recurso.Id == recursoId);
        Assert.True(response2.Recurso.DataInclusao.Ticks == dataInclusao.Ticks);
        Assert.True(response2.Recurso.Senha == null);
        Assert.True(response2.Recurso.Salt == null);
    }
}
