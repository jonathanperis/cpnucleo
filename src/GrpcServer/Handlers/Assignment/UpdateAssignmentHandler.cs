namespace GrpcServer.Handlers.Assignment;

public sealed class UpdateAssignmentHandler : ICommandHandler<UpdateAssignmentCommand, UpdateAssignmentResult>
{
    public async Task<UpdateAssignmentResult> ExecuteAsync(UpdateAssignmentCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}