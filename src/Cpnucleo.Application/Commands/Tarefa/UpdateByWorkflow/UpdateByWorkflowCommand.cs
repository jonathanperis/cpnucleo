namespace Cpnucleo.Application.Commands.Tarefa.UpdateByWorkflow;

public class UpdateByWorkflowCommand : IRequest<OperationResult>
{
    public Guid Id { get; }
    public Guid IdWorkflow { get; }

    public UpdateByWorkflowCommand(Guid id, Guid idWorkflow)
    {
        Id = id;
        IdWorkflow = idWorkflow;
    }
}
