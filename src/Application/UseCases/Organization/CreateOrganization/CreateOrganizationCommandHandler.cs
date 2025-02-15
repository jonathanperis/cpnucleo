namespace Application.UseCases.Organization.CreateOrganization;

public sealed class CreateOrganizationCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<CreateOrganizationCommand, OperationResult>
{
    public async ValueTask<OperationResult> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
    {
        var system = Domain.Entities.Organization.Create(request.Name, request.Description, request.Id);

        if (dbContext.Organizations is not null)
            await dbContext.Organizations.AddAsync(system, cancellationToken);

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        return result ? OperationResult.Success : OperationResult.Failed;
    }
}
