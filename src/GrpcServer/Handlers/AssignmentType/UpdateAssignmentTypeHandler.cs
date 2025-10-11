namespace GrpcServer.Handlers.AssignmentType;

public sealed class UpdateAssignmentTypeHandler : ICommandHandler<UpdateAssignmentTypeCommand, UpdateAssignmentTypeResult>
{
    public async Task<UpdateAssignmentTypeResult> ExecuteAsync(UpdateAssignmentTypeCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}