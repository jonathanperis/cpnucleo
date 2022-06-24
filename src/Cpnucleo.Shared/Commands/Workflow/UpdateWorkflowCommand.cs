namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Workflow;

public record UpdateWorkflowCommand(Guid Id, string Nome, int Ordem) : BaseCommand, IRequest<OperationResult>;