namespace GrpcServer.Services;

internal class AssignmentImpedimentGrpcService(ISender sender) : ServiceBase<IAssignmentImpedimentGrpcService>, IAssignmentImpedimentGrpcService
{
    public async UnaryResult<OperationResult> CreateAssignmentImpediment(CreateAssignmentImpedimentCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<GetAssignmentImpedimentByIdQueryViewModel> GetAssignmentImpedimentById(GetAssignmentImpedimentByIdQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<ListAssignmentImpedimentQueryViewModel> ListAssignmentImpediment(ListAssignmentImpedimentQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveAssignmentImpediment(RemoveAssignmentImpedimentCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateAssignmentImpediment(UpdateAssignmentImpedimentCommand command)
    {
        return await sender.Send(command);
    }
}