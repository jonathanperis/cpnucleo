namespace Application.UseCases.Organization.RemoveOrganization;

public sealed class RemoveOrganizationCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<RemoveOrganizationCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(RemoveOrganizationCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.Organizations is not null)
        {
            var system = await dbContext.Organizations
                .FirstOrDefaultAsync(s => s.Id == request.Id && s.Active, cancellationToken);

            if (system == null)
            {
                return OperationResult.NotFound;
            }

            system = Domain.Entities.Organization.Remove(system);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
