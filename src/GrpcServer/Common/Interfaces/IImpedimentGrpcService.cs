namespace GrpcServer.Common.Interfaces;

public interface IImpedimentGrpcService : IService<IImpedimentGrpcService>
{
    UnaryResult<OperationResult> CreateImpediment(CreateImpedimentCommand command);
    
    UnaryResult<GetImpedimentByIdQueryViewModel> GetImpedimentById(GetImpedimentByIdQuery query);

    UnaryResult<ListImpedimentQueryViewModel> ListImpediment(ListImpedimentQuery query);    
    
    UnaryResult<OperationResult> RemoveImpediment(RemoveImpedimentCommand command);
    
    UnaryResult<OperationResult> UpdateImpediment(UpdateImpedimentCommand command);
}