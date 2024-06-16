namespace Application.UseCases.System.RemoveSystem;

public sealed class RemoveSystemCommandHandler : IRequestHandler<RemoveSystemCommand, OperationResult>
{
    private readonly IApplicationDbContext _dbContext;

    public RemoveSystemCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<OperationResult> Handle(RemoveSystemCommand request, CancellationToken cancellationToken)
    {
        if (_dbContext.Systems is not null)
        {
            var system = await _dbContext.Systems
                .FirstOrDefaultAsync(s => s.Id == request.Id && s.Active, cancellationToken);

            if (system == null)
            {
                return OperationResult.NotFound;
            }

            system = Domain.Entities.System.Remove(system);
        }

        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
