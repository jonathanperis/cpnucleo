using Cpnucleo.Shared.Commands.Workflow;

namespace Cpnucleo.Application.Commands.Workflow;

public class CreateWorkflowCommandValidator : AbstractValidator<CreateWorkflowCommand>
{
    public CreateWorkflowCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.Nome).MaximumLength(50);
        RuleFor(x => x.Ordem).NotEmpty();
        RuleFor(x => x.Ordem).InclusiveBetween(1, 10);
    }
}
