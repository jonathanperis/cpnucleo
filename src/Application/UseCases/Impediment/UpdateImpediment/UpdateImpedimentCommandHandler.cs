namespace Application.UseCases.Impediment.UpdateImpediment;

// EF Core
public sealed class UpdateImpedimentCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateImpedimentCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(UpdateImpedimentCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.Impediments is not null)
        {
            var impediment = await dbContext.Impediments
                .FirstOrDefaultAsync(p => p.Id == request.Id && p.Active, cancellationToken);

            if (impediment is null)
            {
                return OperationResult.NotFound;
            }

            Domain.Entities.Impediment.Update(impediment, request.Name);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
