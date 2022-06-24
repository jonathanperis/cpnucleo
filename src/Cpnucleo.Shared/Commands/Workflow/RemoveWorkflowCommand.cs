using Cpnucleo.Shared.Commands;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.Workflow;

public record RemoveWorkflowCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;