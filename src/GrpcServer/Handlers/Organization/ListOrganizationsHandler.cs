namespace GrpcServer.Handlers.Organization;

public sealed class ListOrganizationsHandler : ICommandHandler<ListOrganizationsCommand, ListOrganizationsResult>
{
    public async Task<ListOrganizationsResult> ExecuteAsync(ListOrganizationsCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}