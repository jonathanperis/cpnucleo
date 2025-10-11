namespace GrpcServer.Handlers.Organization;

public sealed class RemoveOrganizationHandler : ICommandHandler<RemoveOrganizationCommand, RemoveOrganizationResult>
{
    public async Task<RemoveOrganizationResult> ExecuteAsync(RemoveOrganizationCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}