namespace Application.UseCases.System.CreateSystem;

public sealed class CreateSystemCommandHandler : IRequestHandler<CreateSystemCommand, OperationResult>
{
    private readonly IApplicationDbContext _dbContext;

    public CreateSystemCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<OperationResult> Handle(CreateSystemCommand request, CancellationToken cancellationToken)
    {
        var system = Domain.Entities.System.Create(request.Name, request.Description, request.Id);

        if (_dbContext.Systems is not null)
            await _dbContext.Systems.AddAsync(system, cancellationToken);

        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
