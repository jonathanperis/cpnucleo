namespace GrpcServer.Handlers.Organization;

public sealed class GetOrganizationByIdHandler : ICommandHandler<GetOrganizationByIdCommand, GetOrganizationByIdResult>
{
    public async Task<GetOrganizationByIdResult> ExecuteAsync(GetOrganizationByIdCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}