namespace Application.UseCases.Impediment.RemoveImpediment;

public sealed class RemoveImpedimentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<RemoveImpedimentCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(RemoveImpedimentCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.Impediments is not null)
        {
            var impediment = await dbContext.Impediments
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (impediment == null)
            {
                return OperationResult.NotFound;
            }
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
