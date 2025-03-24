namespace GrpcServer.Services;

internal class AssignmentGrpcService(ISender sender) : ServiceBase<IAssignmentGrpcService>, IAssignmentGrpcService
{
    public async UnaryResult<OperationResult> CreateAssignment(CreateAssignmentCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<GetAssignmentByIdQueryViewModel> GetAssignmentById(GetAssignmentByIdQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<ListAssignmentQueryViewModel> ListAssignment(ListAssignmentQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveAssignment(RemoveAssignmentCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateAssignment(UpdateAssignmentCommand command)
    {
        return await sender.Send(command);
    }
}