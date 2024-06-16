namespace Application.UseCases.System.UpdateSystem;

public sealed class UpdateSystemCommandHandler : IRequestHandler<UpdateSystemCommand, OperationResult>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateSystemCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async ValueTask<OperationResult> Handle(UpdateSystemCommand request, CancellationToken cancellationToken)
    {
        if (_dbContext.Systems is not null)
        {
            var system = await _dbContext.Systems
                .FirstOrDefaultAsync(s => s.Id == request.Id && s.Active, cancellationToken);

            if (system == null)
            {
                return OperationResult.NotFound;
            }

            system = Domain.Entities.System.Update(system, request.Name, request.Description);
        }

        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
