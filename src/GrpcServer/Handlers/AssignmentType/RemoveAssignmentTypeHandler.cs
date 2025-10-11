namespace GrpcServer.Handlers.AssignmentType;

public sealed class RemoveAssignmentTypeHandler : ICommandHandler<RemoveAssignmentTypeCommand, RemoveAssignmentTypeResult>
{
    public async Task<RemoveAssignmentTypeResult> ExecuteAsync(RemoveAssignmentTypeCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}