using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Application.Test.Handlers;

public class RecursoProjetoHandlerTest
{
    [Fact]
    public async Task CreateRecursoProjetoCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();
        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

        Guid projetoId = Guid.NewGuid();
        await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));

        Guid workflowId = Guid.NewGuid();
        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));

        Guid recursoId = Guid.NewGuid();
        await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));

        await unitOfWork.SaveChangesAsync();

        CreateRecursoProjetoCommand request = new()
        {
            RecursoProjeto = MockViewModelHelper.GetNewRecursoProjeto(projetoId, recursoId)
        };

        // Act
        RecursoProjetoHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetRecursoProjetoQuery_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();
        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

        Guid projetoId = Guid.NewGuid();
        await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));

        Guid workflowId = Guid.NewGuid();
        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));

        Guid recursoId = Guid.NewGuid();
        await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));

        Guid recursoProjetoId = Guid.NewGuid();
        await unitOfWork.RecursoProjetoRepository.AddAsync(MockEntityHelper.GetNewRecursoProjeto(projetoId, recursoId, recursoProjetoId));

        await unitOfWork.SaveChangesAsync();

        GetRecursoProjetoQuery request = new()
        {
            Id = recursoProjetoId
        };

        // Act
        RecursoProjetoHandler handler = new(unitOfWork, mapper);
        RecursoProjetoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Id != Guid.Empty);
        Assert.True(response.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task ListRecursoProjetoQuery_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();
        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

        Guid projetoId = Guid.NewGuid();
        await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));

        Guid workflowId = Guid.NewGuid();
        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));

        Guid recursoId = Guid.NewGuid();
        await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));

        Guid recursoProjetoId = Guid.NewGuid();
        await unitOfWork.RecursoProjetoRepository.AddAsync(MockEntityHelper.GetNewRecursoProjeto(projetoId, recursoId, recursoProjetoId));
        await unitOfWork.RecursoProjetoRepository.AddAsync(MockEntityHelper.GetNewRecursoProjeto(projetoId, recursoId));
        await unitOfWork.RecursoProjetoRepository.AddAsync(MockEntityHelper.GetNewRecursoProjeto(projetoId, recursoId));

        await unitOfWork.SaveChangesAsync();

        ListRecursoProjetoQuery request = new();

        // Act
        RecursoProjetoHandler handler = new(unitOfWork, mapper);
        IEnumerable<RecursoProjetoViewModel> response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response != null);
        Assert.True(response.Any());
        Assert.True(response.FirstOrDefault(x => x.Id == recursoProjetoId) != null);
    }

    [Fact]
    public async Task RemoveRecursoProjetoCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();
        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

        Guid projetoId = Guid.NewGuid();
        await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));

        Guid workflowId = Guid.NewGuid();
        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));

        Guid recursoId = Guid.NewGuid();
        await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));

        Guid recursoProjetoId = Guid.NewGuid();
        RecursoProjeto recursoProjeto = MockEntityHelper.GetNewRecursoProjeto(projetoId, recursoId, recursoProjetoId);

        await unitOfWork.RecursoProjetoRepository.AddAsync(recursoProjeto);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.RecursoProjetoRepository.Detatch(recursoProjeto);

        RemoveRecursoProjetoCommand request = new()
        {
            Id = recursoProjetoId
        };

        GetRecursoProjetoQuery request2 = new()
        {
            Id = recursoProjetoId
        };

        // Act
        RecursoProjetoHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        RecursoProjetoViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 == null);
    }

    [Fact]
    public async Task UpdateRecursoProjetoCommand_Handle()
    {
        // Arrange
        IUnitOfWork unitOfWork = DbContextHelper.GetContext();
        IMapper mapper = AutoMapperHelper.GetMappings();

        Guid sistemaId = Guid.NewGuid();
        await unitOfWork.SistemaRepository.AddAsync(MockEntityHelper.GetNewSistema(sistemaId));

        Guid projetoId = Guid.NewGuid();
        await unitOfWork.ProjetoRepository.AddAsync(MockEntityHelper.GetNewProjeto(sistemaId, projetoId));

        Guid workflowId = Guid.NewGuid();
        await unitOfWork.WorkflowRepository.AddAsync(MockEntityHelper.GetNewWorkflow(workflowId));

        Guid recursoId = Guid.NewGuid();
        await unitOfWork.RecursoRepository.AddAsync(MockEntityHelper.GetNewRecurso(recursoId));

        Guid recursoProjetoId = Guid.NewGuid();
        DateTime dataInclusao = DateTime.Now;
        RecursoProjeto recursoProjeto = MockEntityHelper.GetNewRecursoProjeto(projetoId, recursoId, recursoProjetoId);

        await unitOfWork.RecursoProjetoRepository.AddAsync(recursoProjeto);
        await unitOfWork.SaveChangesAsync();

        unitOfWork.RecursoProjetoRepository.Detatch(recursoProjeto);

        UpdateRecursoProjetoCommand request = new()
        {
            RecursoProjeto = MockViewModelHelper.GetNewRecursoProjeto(projetoId, recursoId, recursoProjetoId, dataInclusao)
        };

        GetRecursoProjetoQuery request2 = new()
        {
            Id = recursoProjetoId
        };

        // Act
        RecursoProjetoHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);
        RecursoProjetoViewModel response2 = await handler.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2 != null);
        Assert.True(response2.Id == recursoProjetoId);
        Assert.True(response2.DataInclusao.Ticks == dataInclusao.Ticks);
    }
}
