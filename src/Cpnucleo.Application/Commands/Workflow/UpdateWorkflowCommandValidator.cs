namespace Cpnucleo.Application.Commands.Workflow;

public class UpdateWorkflowCommandValidator : AbstractValidator<UpdateWorkflowCommand>
{
    public UpdateWorkflowCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.Nome).MaximumLength(50);
        RuleFor(x => x.Ordem).NotEmpty();
        RuleFor(x => x.Ordem).InclusiveBetween(1, 10);
    }
}
