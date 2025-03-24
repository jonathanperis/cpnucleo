namespace GrpcServer.Services;

internal class AssignmentTypeGrpcService(ISender sender) : ServiceBase<IAssignmentTypeGrpcService>, IAssignmentTypeGrpcService
{
    public async UnaryResult<OperationResult> CreateAssignmentType(CreateAssignmentTypeCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<GetAssignmentTypeByIdQueryViewModel> GetAssignmentTypeById(GetAssignmentTypeByIdQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<ListAssignmentTypeQueryViewModel> ListAssignmentType(ListAssignmentTypeQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveAssignmentType(RemoveAssignmentTypeCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateAssignmentType(UpdateAssignmentTypeCommand command)
    {
        return await sender.Send(command);
    }
}