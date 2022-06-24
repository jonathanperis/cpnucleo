using Cpnucleo.Shared.Commands.Workflow;

namespace Cpnucleo.Application.Commands.Workflow;

public class RemoveWorkflowCommandValidator : AbstractValidator<RemoveWorkflowCommand>
{
    public RemoveWorkflowCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
