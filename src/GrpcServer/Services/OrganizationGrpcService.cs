namespace GrpcServer.Services;

internal class OrganizationGrpcService(ISender sender) : ServiceBase<IOrganizationGrpcService>, IOrganizationGrpcService
{
    public async UnaryResult<OperationResult> CreateOrganization(CreateOrganizationCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<GetOrganizationByIdQueryViewModel> GetOrganizationById(GetOrganizationByIdQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<ListOrganizationQueryViewModel> ListOrganization(ListOrganizationQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveOrganization(RemoveOrganizationCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateOrganization(UpdateOrganizationCommand command)
    {
        return await sender.Send(command);
    }
}