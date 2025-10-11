namespace GrpcServer.Handlers.AssignmentType;

public sealed class CreateAssignmentTypeHandler : ICommandHandler<CreateAssignmentTypeCommand, CreateAssignmentTypeResult>
{
    public async Task<CreateAssignmentTypeResult> ExecuteAsync(CreateAssignmentTypeCommand command, CancellationToken cancellationToken)
    {
        return null;
    }
}