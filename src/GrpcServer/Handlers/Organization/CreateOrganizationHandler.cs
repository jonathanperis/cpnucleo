namespace GrpcServer.Handlers.Organization;

public sealed class CreateOrganizationHandler : ICommandHandler<CreateOrganizationCommand, CreateOrganizationResult>
{
    public async Task<CreateOrganizationResult> ExecuteAsync(CreateOrganizationCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}