namespace GrpcServer.Common.Interfaces;

public interface IOrganizationGrpcService : IService<IOrganizationGrpcService>
{
    UnaryResult<OperationResult> CreateOrganization(CreateOrganizationCommand command);
    
    UnaryResult<GetOrganizationByIdQueryViewModel> GetOrganizationById(GetOrganizationByIdQuery query);

    UnaryResult<ListOrganizationQueryViewModel> ListOrganization(ListOrganizationQuery query);    
    
    UnaryResult<OperationResult> RemoveOrganization(RemoveOrganizationCommand command);
    
    UnaryResult<OperationResult> UpdateOrganization(UpdateOrganizationCommand command);
}