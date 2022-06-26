using Cpnucleo.Application.Commands.RecursoProjeto;
using Cpnucleo.Application.Queries.RecursoProjeto;

namespace Cpnucleo.Application.Test.Handlers;

public class RecursoProjetoHandlerTest
{
    [Fact]
    public async Task CreateRecursoProjetoCommand_Handle_Success()
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

        CreateRecursoProjetoCommand request = MockCommandHelper.GetNewCreateRecursoProjetoCommand(projetoId, recursoId);

        // Act
        CreateRecursoProjetoHandler handler = new(unitOfWork, mapper);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
    }

    [Fact]
    public async Task GetRecursoProjetoQuery_Handle_Success()
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

        GetRecursoProjetoQuery request = MockQueryHelper.GetNewGetRecursoProjetoQuery(recursoProjetoId);

        // Act
        GetRecursoProjetoHandler handler = new(unitOfWork, mapper);
        GetRecursoProjetoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.RecursoProjeto != null);
        Assert.True(response.RecursoProjeto.Id != Guid.Empty);
        Assert.True(response.RecursoProjeto.DataInclusao.Ticks != 0);
    }

    [Fact]
    public async Task GetRecursoProjetoByProjetoQuery_Handle_Success()
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

        GetRecursoProjetoByProjetoQuery request = MockQueryHelper.GetNewGetRecursoProjetoByProjetoQuery(projetoId);

        // Act
        GetRecursoProjetoByProjetoHandler handler = new(unitOfWork, mapper);
        GetRecursoProjetoByProjetoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.RecursoProjetos != null);
        Assert.True(response.RecursoProjetos.Any());
        Assert.True(response.RecursoProjetos.FirstOrDefault(x => x.Id == recursoProjetoId) != null);
    }

    [Fact]
    public async Task ListRecursoProjetoQuery_Handle_Success()
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

        ListRecursoProjetoQuery request = MockQueryHelper.GetNewListRecursoProjetoQuery();

        // Act
        ListRecursoProjetoHandler handler = new(unitOfWork, mapper);
        ListRecursoProjetoViewModel response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(response.RecursoProjetos != null);
        Assert.True(response.RecursoProjetos.Any());
        Assert.True(response.RecursoProjetos.FirstOrDefault(x => x.Id == recursoProjetoId) != null);
    }

    [Fact]
    public async Task RemoveRecursoProjetoCommand_Handle_Success()
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

        RemoveRecursoProjetoCommand request = MockCommandHelper.GetNewRemoveRecursoProjetoCommand(recursoProjetoId);
        GetRecursoProjetoQuery request2 = MockQueryHelper.GetNewGetRecursoProjetoQuery(recursoProjetoId);

        // Act
        RemoveRecursoProjetoHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetRecursoProjetoHandler handler2 = new(unitOfWork, mapper);
        GetRecursoProjetoViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.OperationResult == OperationResult.NotFound);
    }

    [Fact]
    public async Task UpdateRecursoProjetoCommand_Handle_Success()
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

        UpdateRecursoProjetoCommand request = MockCommandHelper.GetNewUpdateRecursoProjetoCommand(projetoId, recursoId, recursoProjetoId);
        GetRecursoProjetoQuery request2 = MockQueryHelper.GetNewGetRecursoProjetoQuery(recursoProjetoId);

        // Act
        UpdateRecursoProjetoHandler handler = new(unitOfWork);
        OperationResult response = await handler.Handle(request, CancellationToken.None);

        GetRecursoProjetoHandler handler2 = new(unitOfWork, mapper);
        GetRecursoProjetoViewModel response2 = await handler2.Handle(request2, CancellationToken.None);

        // Assert
        Assert.True(response == OperationResult.Success);
        Assert.True(response2.RecursoProjeto != null);
        Assert.True(response2.RecursoProjeto.Id == recursoProjetoId);
    }
}
