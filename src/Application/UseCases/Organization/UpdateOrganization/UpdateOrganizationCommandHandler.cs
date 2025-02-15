namespace Application.UseCases.Organization.UpdateOrganization;

public sealed class UpdateOrganizationCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateOrganizationCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
    {
        if (dbContext.Organizations is not null)
        {
            var system = await dbContext.Organizations
                .FirstOrDefaultAsync(s => s.Id == request.Id && s.Active, cancellationToken);

            if (system is null)
            {
                return OperationResult.NotFound;
            }

            Domain.Entities.Organization.Update(system, request.Name, request.Description);
        }

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
