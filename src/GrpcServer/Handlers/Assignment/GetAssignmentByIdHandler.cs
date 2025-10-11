namespace GrpcServer.Handlers.Assignment;

public sealed class GetAssignmentByIdHandler : ICommandHandler<GetAssignmentByIdCommand, GetAssignmentByIdResult>
{
    public async Task<GetAssignmentByIdResult> ExecuteAsync(GetAssignmentByIdCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}