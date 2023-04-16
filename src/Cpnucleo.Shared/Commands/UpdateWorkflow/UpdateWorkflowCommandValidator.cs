namespace Cpnucleo.Shared.Commands.UpdateWorkflow;

public sealed class UpdateWorkflowCommandValidator : AbstractValidator<UpdateWorkflowCommand>
{
    public UpdateWorkflowCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Workflow");

        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Necessário informar o Nome do Workflow");

        RuleFor(x => x.Nome)
            .MaximumLength(50)
            .WithMessage("Nome pode conter no máximo 50 caractéres");

        RuleFor(x => x.Ordem)
            .NotEmpty()
            .WithMessage("Necessário informar a Ordem do Workflow");

        RuleFor(x => x.Ordem)
            .InclusiveBetween(1, 10)
            .WithMessage("Ordem deve estar entre 1 e 10");
    }
}
