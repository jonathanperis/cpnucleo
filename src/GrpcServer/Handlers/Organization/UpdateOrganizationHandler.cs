namespace GrpcServer.Handlers.Organization;

public sealed class UpdateOrganizationHandler : ICommandHandler<UpdateOrganizationCommand, UpdateOrganizationResult>
{
    public async Task<UpdateOrganizationResult> ExecuteAsync(UpdateOrganizationCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}
