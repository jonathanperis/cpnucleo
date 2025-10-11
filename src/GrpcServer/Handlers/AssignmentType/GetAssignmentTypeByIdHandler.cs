namespace GrpcServer.Handlers.AssignmentType;

public sealed class GetAssignmentTypeByIdHandler : ICommandHandler<GetAssignmentTypeByIdCommand, GetAssignmentTypeByIdResult>
{
    public async Task<GetAssignmentTypeByIdResult> ExecuteAsync(GetAssignmentTypeByIdCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}